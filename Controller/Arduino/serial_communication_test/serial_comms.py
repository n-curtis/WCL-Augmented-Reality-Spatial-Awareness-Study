import serial
import time

port = 'COM7'
ard = serial.Serial(port, 9600, timeout = 30)
time.sleep(2)

def write(message):
    # Serial write section
    ard.flush()
    print("Python value sent: \t" + str(message))
    ard.write(message.encode())
    time.sleep(2)

    # Serial read section
    msg = ard.read(ard.inWaiting())
    print(msg.decode())

    time.sleep(4)
    msg = ard.read(ard.inWaiting())
    while (msg.decode() == "standby"):
        msg = ard.read()
        time.sleep(1)
    msg = ard.read(ard.inWaiting())
    print(msg.decode())
