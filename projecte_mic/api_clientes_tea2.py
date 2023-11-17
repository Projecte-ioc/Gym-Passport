from flask import Flask, request, jsonify

from projecte_mic.database_models_tea2 import User
from utils_tea_2 import Connexion

app = Flask(__name__)
db = Connexion()

'''
API PARA PODER PROBAR YO QUE LOS DATOS SE ACTUALIZAN EN LA TABLA users_data.
'''

def get_clients_with_par(filter):
    connex, cursor = db.get_connection_to_db()

    filter_name = request.args.get('nombre')
    filter_id = request.args.get('id')
    filter_user = request.args.get('usuario')

    query_sql = f"SELECT * FROM {User.__table_name__} WHERE 1=1"
    if filter_id:
        query_sql += f" AND id = '{filter_id}'"
    if filter_name:
        query_sql += f" AND nombre = '{filter_name}'"
    if filter_user:
        query_sql += f" AND usuario = '{filter_user}'"
    cursor.execute(query_sql)
    records = cursor.fetchall()
    connex.close()

    return records


# DEVUELVE LOS VALORES EN JSON EN FORMATO LLAVE - VALOR
def format_records(records, column_names):
    formatted_records = []
    for record in records:
        formatted_record = {column_names[i]: record[i] for i in range(len(column_names))}
        formatted_records.append(formatted_record)
    return formatted_records


@app.route('/clientes', methods=['GET'])
def get_all_clientes():
    connex, cursor = db.get_connection_to_db()
    cursor.execute(f'SELECT * FROM {User.__table_name__}')
    records = cursor.fetchall()
    column_names = [desc[0] for desc in cursor.description]
    connex.close()
    formatted_records = format_records(records, column_names)
    return jsonify(formatted_records)


@app.route('/cliente', methods=['GET'])
def get_clients_with_params():
    '''
    EndPoint que retorna les dades de un client segons els paramentres que li passem.
    Exemple d'ús:
        http://http://10.2.190.11:5000/cliente?name=Meritxell
    '''
    connex, cursor = db.get_connection_to_db()

    filter_name = request.args.get('name')
    filter_id = request.args.get('id')
    filter_user = request.args.get('user_name')

    query_sql = f"SELECT * FROM {User.__table_name__} WHERE 1=1"
    if filter_id:
        query_sql += f" AND id = '{filter_id}'"
    if filter_name:
        query_sql += f" AND name = '{filter_name}'"
    if filter_user:
        query_sql += f" AND user_name = '{filter_user}'"
    cursor.execute(query_sql)
    records = cursor.fetchall()
    column_names = [desc[0] for desc in cursor.description]
    connex.close()
    formatted_records = format_records(records, column_names)
    return jsonify(formatted_records)


if __name__ == '__main__':
    app.run(host="0.0.0.0", port=5000)
