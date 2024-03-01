from flask import Flask
import json
import random
import os

app = Flask(__name__)


def getDir():
    return os.path.dirname(os.path.realpath(__file__))+ '/'


@app.route('/')
def hello():
    return 'Hello, World!'



@app.route('/get_rdm_question')
def get_rdm_question():
    language = 'en'
    
    with open(f'{getDir()}{language}_questions.json') as f:
        questions = json.load(f)
    
    question = random.choice(questions)
    return question



if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5050, debug=True)