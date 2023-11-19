from flask import Flask, request, jsonify

from database_models_tea2 import User, GymEvent
from utils_tea_2 import Connexion

app = Flask(__name__)
db = Connexion()


#  __keys_events__ = ['id', 'name', 'whereisit', 'schedule', 'qty_max_attendes', 'qty_got_it', 'user_id',
#                      'gym_id', 'done','date', 'hour']
# TODO revisar error 'unhasable type: list'
@app.route('/obtener_eventos')
def get_all_events():
    token = request.headers.get('Authorization')
    rol_user, id, user_name, gym_name = db.validate_rol_user(token)
    results = db.get_elements_filtered(id, GymEvent.__table_name__, 'gym_id', '*')
    if results:
        print(type(results))
        # results_dict = [dict(zip(GymEvent.__keys_events__, row)) for row in results]
        print(str(results))
        return jsonify({results}), 200
    else:
        return jsonify({'message': 'No es possible recuperar les dades'}), 404


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=6000)
