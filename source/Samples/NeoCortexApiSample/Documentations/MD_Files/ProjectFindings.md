# Project Findings: ML 24/25-01 Investigate Image Reconstruction by using Classifiers
 
 Here is a result with 50 different images. The Spatial Pooler has reached the stable state after 62 cycles.

#### Input Image:
One of the input images that has been used in this experiment.

| ![Image 5](https://github.com/user-attachments/assets/5bf5cc2b-7d06-41b3-9195-656a248542af)
|:--:| 
| *Fig 1: Input Image* |

#### Binarized Image:
After Binarization this image represents the binarized version of the input image.
| ![image](https://github.com/user-attachments/assets/71fb2135-c1c4-4d2a-ab75-ddc617600825)
|:--:| 
| *Fig 2: Binarized Image* |

#### Stable SDRs:
The Spatial Pooler reached stable state after iterating 62 cycles.

| ![Stable sdr](https://github.com/user-attachments/assets/ba973bf8-140a-4e48-a4da-f58037040221)
|:--:| 
| *Fig 3: Stable SDRs generated after 62 cycles* |

#### Reconstructed Image:
Image reconstructed by the classifiers.

| ![image](https://github.com/user-attachments/assets/52fb5a38-c353-4694-8dcf-dcc7205595b4)
|:--:| 
| *Fig 4: Reconstructed Image* |
                                         
#### CASE 1: 
HTM Classifiers performed better via predicting the Image with higher similarity. Also the classifier Prediction and Reconstruction time is being captured and the similarity between both the reconstructed images are presented. 
 
| ![Figure 1](https://github.com/user-attachments/assets/7704b32b-8307-4c26-b636-801591ca62d6)
|:--:| 
| *Fig 5: Case where both the classifiers predicted different images* |

 #### CASE 2: 
 This is scenario where both the HTM and KNN Classifiers predicted the same image. So we can say that here both the classifiers performed equally.
 
| ![Ssme image](https://github.com/user-attachments/assets/f3cf45ca-cd17-4b7b-9892-b8e3e9c3ac3d)
|:--:| 
| *Fig 6: Case where both the classifiers predicted same image* |

 #### Classifier Performance :
 Overall for all the predictions and reconstructions we calculated which classifier performed better. Here we can see that HTM Classifier outperformed KNN Classifier.

| ![Outperforming ](https://github.com/user-attachments/assets/c65cc038-3562-4e7d-81cf-ed58d83c1e48)
|:--:| 
| *Fig 7: HTM Classifier outperformed KNN Classifier* |
 
 For both the classifiers all the predicted images are reconstructed and the similarity of the Reconstructed Images with the Original Binarized Images are calculated. These similarities are taken into account via two methods - Jaccard Index Similarity (The JaccardSimilarityofBinaryArrays method calculates the similarity between two binary arrays by dividing the count of shared 1s (intersection) by the count of total 1s present in either array (union), producing a value between 0 and 1 to measure their resemblance) and Hamming Distance Similarity (The GetHammingDistance method calculates the similarity between two binary matrices by counting differing bits for each row, optionally considering only nonzero bits, and computes the percentage similarity based on the total bit count, returning an array of similarity scores). The Jaccard Index Similarities are visualized in BarGraph, similarly the Hamming Distance Similarities are visualized in ScottPlot. 
 
 #### Visualizations For HTM:
 
| ![WhatsApp Image 2025-03-27 at 20 35 41_3fde3a7a](https://github.com/user-attachments/assets/229c5d92-38d3-4ec4-9e92-850cd6633239)
|:--:| 
| *Fig 8: BarGraph illustrating Jaccard Similarity of the HTM Reconstructed Images with the Original Binarized Images* |

| ![WhatsApp Image 2025-03-27 at 20 36 06_4bd76e78](https://github.com/user-attachments/assets/d72306ec-57fa-4d08-b6ae-09b855bc1649)
|:--:| 
| *Fig 9: Scott Plot illustrating Hamming Distance Similarity of the HTM Reconstructed Images with the Original Binarized Images* |
 
 #### Visualizations For KNN:
 
| ![WhatsApp Image 2025-03-27 at 20 36 38_c44a1394](https://github.com/user-attachments/assets/634af39f-ea73-4eae-9c89-7dbd0c090241)
|:--:| 
| *Fig 10: BarGraph illustrating Jaccard Similarity of the KNN Reconstructed Images with the Original Binarized Images* |

| ![WhatsApp Image 2025-03-27 at 20 36 53_68044109](https://github.com/user-attachments/assets/3ef1c35f-6ca3-4946-99c9-74bc71425c19)
|:--:| 
| *Fig 11: Scott Plot illustrating Hamming Distance Similarity of the KNN Reconstructed Images with the Original Binarized Images* |

