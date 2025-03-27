# Project Findings: ML 24/25-01 Investigate Image Reconstruction by using Classifiers
 
 This is a result with 50 different images. The Spatial Pooler has reached the stable state after 62 cycles.
 
#### CASE 1: 
HTM Classifiers performed better via predicting the Image with higher similarity. Also the classifier Prediction and Reconstruction time is being captured and the similarity between both the reconstructed images are presented. 
 
 ![WhatsApp Image 2025-03-27 at 20 33 37_05364955](https://github.com/user-attachments/assets/0dbd0a53-4581-43fb-a751-a431f5b16566)

 #### CASE 2: 
 This is scenario where both the HTM and KNN Classifiers predicted the same image. So we can say that here both the classifiers performed equally.
 
 ![WhatsApp Image 2025-03-27 at 20 33 25_b0360ad8](https://github.com/user-attachments/assets/0d0e98ed-e09e-43ba-9871-15568fa2c5d4)

 #### Classifier Performance :
 Overall for all the predictions and reconstructions we calculated which classifier performed better. We can see here HTM Classifier outperformed KNN Classifier.

 ![WhatsApp Image 2025-03-27 at 20 34 03_cafabdf8](https://github.com/user-attachments/assets/b50012ab-8a4e-4569-8bf0-acc30f162324)
 
 For both the classifiers all the predicted images are reconstructed and the similarity of the Reconstructed Images with the Original Binarized Images are calculated. These similarities are taken into account via two methods - Jaccard Index Similarity (The JaccardSimilarityofBinaryArrays method calculates the similarity between two binary arrays by dividing the count of shared 1s (intersection) by the count of total 1s present in either array (union), producing a value between 0 and 1 to measure their resemblance) and Hamming Distance Similarity (The GetHammingDistance method calculates the similarity between two binary matrices by counting differing bits for each row, optionally considering only nonzero bits, and computes the percentage similarity based on the total bit count, returning an array of similarity scores). The Jaccard Index Similarities are visualized in BarGraph, similarly the Hamming Distance Similarities are visualized in ScottPlot. 
 
 #### For HTM:
 
 *BarGraph illustrating Jaccard Similarity of the Reconstructed Images with the Original Binarized Images*
 ![WhatsApp Image 2025-03-27 at 20 35 41_3fde3a7a](https://github.com/user-attachments/assets/229c5d92-38d3-4ec4-9e92-850cd6633239)

 *Scott Plot illustrating Hamming Distance Similarity of the Reconstructed Images with the Original Binarized Images*
 ![WhatsApp Image 2025-03-27 at 20 36 06_4bd76e78](https://github.com/user-attachments/assets/d72306ec-57fa-4d08-b6ae-09b855bc1649)
 
 #### For KNN:
 
 *BarGraph illustrating Jaccard Similarity of the Reconstructed Images with the Original Binarized Images*
 ![WhatsApp Image 2025-03-27 at 20 36 38_c44a1394](https://github.com/user-attachments/assets/634af39f-ea73-4eae-9c89-7dbd0c090241)
 
 *Scott Plot illustrating Hamming Distance Similarity of the Reconstructed Images with the Original Binarized Images*
 ![WhatsApp Image 2025-03-27 at 20 36 53_68044109](https://github.com/user-attachments/assets/3ef1c35f-6ca3-4946-99c9-74bc71425c19)
