#  Copyright 2016 Nicholas Curtis
#  Licensed under the Apache License, Version 2.0 (the "License");
#  you may not use this file except in compliance with the License.
#  You may obtain a copy of the License at
#      http://www.apache.org/licenses/LICENSE-2.0
#  Unless required by applicable law or agreed to in writing, software
#  distributed under the License is distributed on an "AS IS" BASIS,
#  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#  See the License for the specific language governing permissions and
#  limitations under the License.

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
