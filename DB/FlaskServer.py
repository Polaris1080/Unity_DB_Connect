from   flask        import abort, Flask, jsonify, make_response, Response, request, url_for
from   pymongo      import MongoClient
from   urllib.parse import urlparse
import configparser, datetime, json, mysql.connector, random, redis

# MongoDBへの接続
def connectMongoDB(_collection,host='localhost',port=27017,db='main'):
    client     = MongoClient(host=host,port=port)
    database   = client  [db]
    collection = database[_collection]
    return collection
# redisへの接続
def connectRedis(db,host='localhost',port=6379):
    pool       = redis.ConnectionPool(host=host,port=port,db=db)
    connection = redis.StrictRedis   (connection_pool=pool)
    return connection

# インスタンスの作成
api = Flask(__name__)

# DBからユーザー情報を読み込む
@api.route('/Unity/DB/<int:Number>', methods=['GET'])
def LoadDB(Number):
    collection = connectMongoDB('user')           # MongoDBへ接続
    read = collection.find_one({"userid":Number}) # 読み込み
    if read != None: # 結果が"None"で無ければ、結果を返信する。
        return make_response(json.dumps({"name":read["name"],"block":read["block"]}, indent=4))
    else:            # もしも結果が"None"ならば、404を返す。
        return make_response(jsonify({'error': 'Not found'}), 404)
# DB上のユーザー情報を更新する
@api.route('/Unity/DB/<int:Number>', methods=['PUT'])
def UpdateDB(Number):
    collection = connectMongoDB('user') # MongoDBへ接続
    # DBに書き込むデータを生成
    write = { "userid": Number,
              "name"  : request.json["name"],
              "block" : request.json["block"] }
    collection.update({"userid":Number}, write)          # DBに書き込む
    return make_response(jsonify({"status": 'OK'}), 200) # 結果を返信
# LOGにブロック破壊数を追記
@api.route('/Unity/log', methods=['POST'])
def AddBlockLOG():
    _id = request.json["id"];   add = request.json["add"]     # POSTされたJSONを読み込み
    write = {"add":add,"datetime":datetime.datetime.utcnow()} # LOGに書き込むJSONを生成
    connectRedis(0).rpush(str(_id), write)                    # LOGに書き込む
    return make_response(jsonify({"status": 'OK'}), 200)      # 結果を返信

# エラーハンドリング
@api.errorhandler(404)
def not_found(error):
    return make_response(jsonify({'error': 'Not found'}), 404)

# サーバー起動　[IP:127.0.0.1 port:8001]
if __name__ == '__main__':
    api.run(host='127.0.0.1', port=8000)