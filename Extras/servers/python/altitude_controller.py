'''
Simple altitude-hold controller based on APM

Copyright (C) 2019 Simon D. Levy

MIT License
'''


class AltitudeController(object):

    def __init__(self, target, posP, velP, velI, windupMax=10):

        # In a real PID controller, this would be a set-point
        self.target = target

        # Constants
        self.posP = posP
        self.velP = velP
        self.velI = velI
        self.windupMax = windupMax

        # Values modified in-flight
        self.lastError = 0
        self.integralError = 0

    def u(self, alt, vel, dt):

        # Compute dzdt setpoint and error
        velTarget = (self.target - alt) * self.posP
        velError = velTarget - vel

        # Update error integral and error derivative
        self.integralError += velError * dt
        self.integralError = AltitudeController._constrainAbs(
                        self.integralError + velError * dt, self.windupMax)
        deltaError = ((velError - self.lastError) / dt
                      if abs(self.lastError) > 0
                      else 0)
        self.lastError = velError

        # Compute control u
        return self.velP * velError + self.velI * self.integralError

    def _constrainAbs(x, lim):

        return -lim if x < -lim else (+lim if x > +lim else x)