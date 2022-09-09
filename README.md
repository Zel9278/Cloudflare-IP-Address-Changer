# Cloudflare IP Address Changer

IPv4 アドレスを自動で取得し，Cloudflare の設定を自動変更するツールです。

# インストール方法

## ビルド

1. リポジトリを任意のディレクトリにクローン
2. [Visual Studio 2019](https://visualstudio.microsoft.com/ja/downloads/) 以降を使用してビルド
3. 完了

## 使い方

1. ビルドすると生成される `CP IP Address Changer.exe.config` を編集する
   - 14 行目: ドメインページの右側，API ゾーン ID
   - 17 行目: 対象の DNS レコード
   - 20 行目: アカウントのメールアドレス
   - 23 行目: API キー
     - API キーは以下の手順で入手できます
       1. アカウントページの `マイプロフィール (My Profile) `
       2. 左側にある `API トークン (API Tokens)`
       3. 下にある `API キー (API Key)`
       4. `Global Key` をコピー
2. `CP IP Address Changer.exe` を実行
