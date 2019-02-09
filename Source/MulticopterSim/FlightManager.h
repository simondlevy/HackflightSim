/*
* FlightManager.h: Abstract flight-management class for MulticopterSim
*
* Copyright (C) 2019 Simon D. Levy
*
* MIT License
*/


#pragma once

class MULTICOPTERSIM_API FlightManager {

public:

    virtual void update(void) = 0;

    /**
     *  Factory method.
     *  @return pointer to a new FlightManager object
     */
     static FlightManager * createFlightManager(void);
};
