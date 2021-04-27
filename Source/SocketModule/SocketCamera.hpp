/*
 * Abstract camera class for MulticopterSim using socket communication
 *
 * Copyright (C) 2021 Simon D. Levy
 *
 * MIT License
 */

#pragma once

#include "../MainModule/Camera.hpp"

#include "../../Extras/sockets/UdpClientSocket.hpp"

class SocketCamera : public Camera {

    private:

        // Comms
        static constexpr char * HOST = "127.0.0.1"; // localhost
        static constexpr uint16_t IMAGE_PORT = 5002;

        // Camera params
        static constexpr Resolution_t RES = RES_640x480;
        static constexpr float FOV = 135;
        static constexpr uint16_t ROWS = 480;
        static constexpr uint16_t COLS = 640;
        static constexpr uint16_t STRIP_HEIGHT = 20;

        // Create one-way server for images out
        UdpClientSocket imageUdp = UdpClientSocket(HOST, IMAGE_PORT);

    public:

        SocketCamera()
            : Camera(FOV, RES)
        {
        }

    protected:

        virtual void processImageBytes(uint8_t * bytes) override
        { 
            // Send image data
            for (uint16_t row=0; row<ROWS; row+=STRIP_HEIGHT) {
                imageUdp.sendData(&bytes[row*STRIP_HEIGHT*4], STRIP_HEIGHT*COLS*4);
            }
        }

}; // Class SocketCamera
