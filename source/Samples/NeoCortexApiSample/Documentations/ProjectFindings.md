# Project Findings: ML 24/25-01 Investigate Image Reconstruction by using Classifiers
 
 This is a result with 50 different images. The Spatial Pooler has reached the stable state after 62 cycles.
 
#### CASE 1: 
HTM Classifiers performed better via predicting the Image with higher similarity. Also the classifier Prediction and Reconstruction time is being captured and the similarity between both the reconstructed images are presented. 
 
 ![WhatsApp Image 2025-03-24 at 18 35 15](https://github.com/user-attachments/assets/6ebc2185-5440-4acf-bc1b-ec78c9e83fde)

 #### CASE 2: 
 This is scenario where both the HTM and KNN Classifiers predicted the same image. So we can say that here both the classifiers performed equally.
 
 ![WhatsApp Image 2025-03-24 at 19 01 44](https://github.com/user-attachments/assets/886a87f7-2e35-4281-811c-5e45028a51ee)

 #### Classifier Performance :
 Overall for all the predictions and reconstructions we calculated which classifier performed better. We can see here HTM Classifier outperformed KNN Classifier.
 
 ![Screenshot 2025-03-25 202614](https://github.com/user-attachments/assets/84c20e48-f290-422a-8093-8b76380f13a8)
 
 For both the classifiers all the predicted images are reconstructed and the similarity of the Reconstructed Images with the Original Binarized Images are calculated. These similarities are taken into account via two methods - Jaccard Index Similarity (The JaccardSimilarityofBinaryArrays method calculates the similarity between two binary arrays by dividing the count of shared 1s (intersection) by the count of total 1s present in either array (union), producing a value between 0 and 1 to measure their resemblance) and Hamming Distance Similarity (The GetHammingDistance method calculates the similarity between two binary matrices by counting differing bits for each row, optionally considering only nonzero bits, and computes the percentage similarity based on the total bit count, returning an array of similarity scores). The Jaccard Index Similarities are visualized in BarGraph, similarly the Hamming Distance Similarities are visualized in ScottPlot. 
 
 #### For HTM:
 
 *BarGraph illustrating Jaccard Similarity of the Reconstructed Images with the Original Binarized Images*
 ![WhatsApp Image 2025-03-24 at 19 15 37](https://github.com/user-attachments/assets/7f4d6b00-2323-4ed2-b438-48a444ca644a)
 
 *Scott Plot illustrating Hamming Distance Similarity of the Reconstructed Images with the Original Binarized Images*
 ![WhatsApp Image 2025-03-24 at 19 16 01](https://github.com/user-attachments/assets/f9ed46ed-6819-4e8b-b133-c4dc538687fb)
 
 #### For KNN:
 
 *BarGraph illustrating Jaccard Similarity of the Reconstructed Images with the Original Binarized Images*
 ![WhatsApp Image 2025-03-24 at 19 19 41](https://github.com/user-attachments/assets/a0fe2536-a67c-490d-8241-3ebdf1d1fb2b)
 
 *Scott Plot illustrating Hamming Distance Similarity of the Reconstructed Images with the Original Binarized Images*
 ![WhatsApp Image 2025-03-24 at 19 20 11](https://github.com/user-attachments/assets/0f873e55-0182-4687-a37d-c1bd0ad61eac)
