from flask import Flask, request, jsonify
import psycopg2

app = Flask(__name__)

# Configura los parámetros de conexión a la base de datos
db_params = {
    'dbname': 'gympassportdb',
    'user': 'mic_ad',
    'password': 'Meri120290!',
    'host': 'gympassportser.postgres.database.azure.com',
    'port': '5432',
}


# Ruta para obtener todos los registros
@app.route('/api/clientes', methods=['GET'])
def get_registros():
    conn = psycopg2.connect(**db_params)
    cursor = conn.cursor()
    cursor.execute("SELECT * FROM clientes")
    registros = cursor.fetchall()
    conn.close()

    # Crear una respuesta JSON
    response = jsonify(registros)

    # Agregar un encabezado personalizado a la respuesta
    response.headers[
        'Ocp-Apim-Subscription-Key'] = '06c0a1a32f8e4d1b9f85c860fb7584ad'  # Reemplaza 'your_subscription_key' con tu valor real

    return response


# Ruta para crear un nuevo registro
@app.route('/api/clientes', methods=['POST'])
def crear_registro():
    data = request.json
    conn = psycopg2.connect(**db_params)
    cursor = conn.cursor()
    cursor.execute("INSERT INTO clientes (nombre, direccion, telefono, horario) VALUES (%s, %s, %s, %s)",
                   (data['nombre'], data['direccion'], data['telefono'], data['horario']))
    conn.commit()
    conn.close()
    return "Registro creado", 201


# Otras rutas para actualizar y eliminar registros

if __name__ == '__main__':
    app.run(debug=True)
