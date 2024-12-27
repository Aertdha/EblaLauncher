import threading
from app import app
from desktop.main import run_desktop_app
from werkzeug.serving import make_server

class ServerThread(threading.Thread):
    def __init__(self, app):
        threading.Thread.__init__(self)
        self.srv = make_server('127.0.0.1', 5000, app)
        self.ctx = app.app_context()
        self.ctx.push()

    def run(self):
        self.srv.serve_forever()

    def shutdown(self):
        self.srv.shutdown()

def start_server():
    global server
    server = ServerThread(app)
    server.start()

if __name__ == '__main__':
    start_server()
    run_desktop_app()
    server.shutdown() 