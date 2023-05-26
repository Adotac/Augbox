from cvzone.HandTrackingModule import HandDetector
import cv2
import socket
import numpy as np

HEIGHT = 480
WIDTH = 400

flag = True

cap = cv2.VideoCapture(0)
cap.set(3, WIDTH)
cap.set(4, HEIGHT)
success, img = cap.read()
h, w, _ = img.shape
detector = HandDetector(detectionCon=0.6, maxHands=2)

udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
tcp_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

udp_sap = ("127.0.0.1", 5052) 
tcp_sap = ("127.0.0.1", 5053) 

udp_socket.bind(udp_sap)
tcp_socket.bind(tcp_sap)

while flag:
    # Wait for TCP connection
    tcp_socket.listen(1)
    print("Waiting for TCP connection...")
    tcp_client, tcp_client_address = tcp_socket.accept()
    print("TCP connection established:", tcp_client_address)

    while True:
        # Get image frame
        success, img = cap.read()

        # Find the hand and its landmarks
        hands, img = detector.findHands(img)  # with draw

        data = []
        if hands:
            # Hand 1
            hand1 = hands[0]
            lmList1 = hand1["lmList"]  # List of 21 Landmark points
            bbox1 = hand1["bbox"][2:4]

            lm = lmList1[0]
            data.extend([hand1["type"], lm[0], h - lm[1], lm[2], w, h, bbox1[0], bbox1[1]])

            if hands.__len__() > 1:
                # Hand 2
                hand2 = hands[1]
                lmList2 = hand2["lmList"]  # List of 21 Landmark points
                bbox2 = hand2["bbox"][2:4]

                lm2 = lmList2[0]
                data.extend([hand2["type"], lm2[0], h - lm2[1], lm2[2], w, h, bbox2[0], bbox2[1]])

            udp_socket.sendto(str.encode(str(data)), udp_sap)

        # Convert image frame to JPEG
        _, encoded_img = cv2.imencode('.jpg', img)
        encoded_img_bytes = np.array(encoded_img).tobytes()

        try:
            # Concatenate size and image data
            message = len(encoded_img_bytes).to_bytes(4, 'big') + encoded_img_bytes

            # Send the message over TCP
            tcp_client.sendall(message)

        except (ConnectionResetError, ConnectionAbortedError):
            # Connection reset or forcibly closed
            print("TCP connection reset or forcibly closed")
            break

        # cv2.imshow("Image", img)
        if cv2.waitKey(1) == ord('/'):
            flag = False
            break

    tcp_client.close()

cap.release()
cv2.destroyAllWindows()
udp_socket.close()
tcp_socket.close()
