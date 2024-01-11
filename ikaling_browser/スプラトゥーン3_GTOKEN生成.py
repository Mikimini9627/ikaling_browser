import sys
import requests
import json
import os

DEBUG = True

A_VERSION = "0.5.7"
F_GEN_URL = "https://api.imink.app/f"
APP_USER_AGENT = 'Mozilla/5.0 (Linux; Android 11; Pixel 5) ' \
                'AppleWebKit/537.36 (KHTML, like Gecko) ' \
                'Chrome/94.0.4606.61 Mobile Safari/537.36'

# 設定ファイルパスを生成
CONFIG_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "config.json")

def generate_header(bullet_token, lang, country):
    import iksm
    return {
            'Authorization':    f'Bearer {bullet_token}', # update every time it's called with current global var
            'Accept-Language':  lang,
            'User-Agent':       APP_USER_AGENT,
            'X-Web-View-Ver':   iksm.get_web_view_ver(),
            'Content-Type':     'application/json',
            'Accept':           '*/*',
            'Origin':           iksm.SPLATNET3_URL,
            'X-Requested-With': 'com.nintendo.znca',
            'Referer':          f'{iksm.SPLATNET3_URL}?lang={lang}&na_country={country}&na_lang={lang}',
            'Accept-Encoding':  'gzip, deflate'
        }

def generate_token(reason = "", session_token = ""):

        import iksm

        if reason != "expiry":

            # ログイン(セッショントークンを取得)
            session_token = iksm.log_in(A_VERSION, APP_USER_AGENT)

        if session_token == None or session_token == "skip":
            sys.exit(1)

        # アカウント情報を取得する
        gtoken, _, lang, country = iksm.get_gtoken(F_GEN_URL, session_token, A_VERSION)

        # バレットトークン取得
        bullet_token = iksm.get_bullet(gtoken, APP_USER_AGENT, lang, country)

        # ファイル書込み
        with open(CONFIG_PATH, "w") as f:
            f.write(json.dumps(
                {
                    "GTOKEN" : gtoken, 
                    "BULLET_TOKEN" : bullet_token,
                    "SESSION_TOKEN" : session_token,
                    "LANGUAGE" : lang,
                    "COUNTRY" : country
                }))

        return gtoken, bullet_token, session_token, lang, country

def check_connection(gtoken, bullet_token, lang, country):
    '''
    疎通確認
    '''

    import utils
    import iksm

    # 疎通確認
    test = requests.post(iksm.GRAPHQL_URL, data=utils.gen_graphql_body(utils.translate_rid["HomeQuery"]), headers=generate_header(bullet_token, lang, country), cookies=dict(_gtoken=gtoken))

    return test.status_code == 200

def main():

    try:

        # 設定ファイルが存在する場合はトークンを読み出す
        gtoken = ""
        if os.path.exists(CONFIG_PATH) == True:

            # ファイル読み込み
            with open(CONFIG_PATH, "r") as f:
                json_data = json.load(f)

            # トークン等読み出し
            gtoken = json_data["GTOKEN"]
            bullet_token = json_data["BULLET_TOKEN"]
            lang = json_data["LANGUAGE"]
            country = json_data["COUNTRY"]
            session_token = json_data["SESSION_TOKEN"]

        # ログインに必要な情報が欠けている場合はトークンを生成する
        if gtoken == "":
            gtoken, bullet_token, session_token, lang, country = generate_token()

        # 期限切れの場合は再度トークン生成
        if check_connection(gtoken, bullet_token, lang, country) == False:

            # 再生成
            gtoken, bullet_token, session_token, lang, country = generate_token("expiry", session_token)

            # 期限切れの場合は再度トークン生成
            if check_connection(gtoken, bullet_token, lang, country) == False:
                print("1")
                exit()

    except:
        print("1")
        import traceback
        print(traceback.format_exc())
        exit()

    # 正常終了
    print("0")

main()
