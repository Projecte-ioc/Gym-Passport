from flask import Flask, request, jsonify
import psycopg2

app = Flask(__name__)

# Configura los parámetros de conexión a la base de datos
db_params = {
    'dbname': 'gympassportdb',
    'user': 'mic_ad',
    'password': 'Meri120290!',
    'host': 'gympassportserv.postgres.database.azure.com',
    'port': '5432',
    'sslmode': 'require'
}


# Ruta para obtener todos los registros
@app.route('/api/registros', methods=['GET'])
def get_registros():
    conn = psycopg2.connect(**db_params)
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM gimnasios")
    registros = cursor.fetchall()
    conn.close()
    return jsonify(registros)


# Ruta para crear un nuevo registro
@app.route('/api/registros', methods=['POST'])
def crear_registro():
    data = request.json
    conn = psycopg2.connect(**db_params)
    cursor = conn.cursor()
    cursor.execute("INSERT INTO gimnasios (nombre, direccion, telefono, horario) VALUES (%s, %s, %s, %s)", (data['nombre'], data['direccion'], data['telefono'], data['horario']))
    conn.commit()
    conn.close()
    return "Registro creado", 201


# Otras rutas para actualizar y eliminar registros

if __name__ == '__main__':
    app.run(debug=True)
