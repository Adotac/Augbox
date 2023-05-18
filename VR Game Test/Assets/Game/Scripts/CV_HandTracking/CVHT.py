from cvzone.HandTrackingModule import HandDetector
import cv2
import socket

HEIGHT = 1270
WIDTH = 720

cap = cv2.VideoCapture(0)
cap.set(3, WIDTH)
cap.set(4, HEIGHT)
success, img = cap.read()
h, w, _ = img.shape
detector = HandDetector(detectionCon=0.6, maxHands=2)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sap1 = ("127.0.0.1", 5052) #left
# sap2 = ("127.0.0.1", 5053) #right

while True:
    # Get image frame
    success, img = cap.read()
    # Find the hand and its landmarks
    hands, img = detector.findHands(img)  # with draw
    # hands = detector.findHands(img, draw=False)  # without draw

    data = []
    # print(hands)
    if hands:
        # Hand 1
        
        hand1 = hands[0]

        lmList1 = hand1["lmList"]  # List of 21 Landmark points
        bbox1 = hand1["bbox"][2:4]

        # for lm in lmList1:\
        lm = lmList1[0]
        data.extend([hand1["type"], lm[0], h - lm[1], lm[2], w, h, bbox1[0], bbox1[1]])

        # sock.sendto(str.encode(str(data)), sap1)
        
        if hands.__len__() > 1:
            # Hand 2
            hand2 = hands[1]
            lmList2 = hand2["lmList"]  # List of 21 Landmark points
            # for lm in lmList2:
            bbox2 = hand2["bbox"][2:4]

            lm2 = lmList2[0]
            data.extend([hand2["type"], lm2[0], h - lm2[1], lm2[2], w, h, bbox2[0], bbox2[1]])

        sock.sendto(str.encode(str(data)), sap1)
        # print(data)
            

    

 


    # Display
    cv2.imshow("Image", img)
    if cv2.waitKey(1) == ord('/'):
        break