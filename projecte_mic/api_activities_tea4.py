import psycopg2
from flask import Flask, request, jsonify

from database_models_tea2 import User, GymEvent
from utils_tea_2 import Connexion

app = Flask(__name__)
db = Connexion()

# __keys_user__ = ['id', 'gym_id', 'name', 'qty_max_attendes', 'qty_got_it', 'schedule']

# TODO GET ACTIVITIES QUE SEAN PARA EL DÍA SIGUIENTE AL QUE NOS ENCONTRAMOS.

# TODO GET ACTIVITIES QUE SEAN DE TODO EL MES(NUMERAL).

# TODO UPDATE list_user_activities_registration SIEMPRE Y CUANDO LA RESERVA DE LA ACTIVIDAD SEA DE HOY PARA MAÑANA.

# TODO UPDATE list_ranking_attending SIEMPRE Y CUANDO EL USUARIO NO SEA ADMIN Y LA DEFERENCIA ENTRE LA FECHA DE LA
# ÚLTIMA ASISTENCIA CON EL DIA ACTUAL SEA IGUAL A 1, SUMAREMOS 1 A LA COLUMNA RACHA.
