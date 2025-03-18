# Project Title: ML 24/25-01 Investigate Image Reconstruction by using Classifiers

## Table of Contents

- [Problem Statement](#problem-statement)
- [Introduction](#introduction)
- [The Overview of the Project](#the-overview-of-the-project)
- [Image Encoder](#image-encoder)
- [Encoding Process](#encoding-process)
- [Sparse Distributed Representations (SDR)](#sparse-distributed-representations-sdr)
- [Hierarchical Temporal Memory (HTM)](#hierarchical-temporal-memory-htm)
- [Spatial Pooler (SP)](#spatial-pooler-sp)
  - [Spatial Pooler Functions](#spatial-pooler-functions)
  - [Phases of the Spatial Pooler](#phases-of-the-spatial-pooler)
- [HTM Classifiers](#htm-classifiers)
- [KNN Classifiers](#knn-classifiers)
- [Difference between HTM and KNN Classifiers](#difference-between-htm-and-knn-classifiers)
- [Reconstruction of Images](#reconstruction-of-images)
- [Methodology](#methodology)
- [Project Findings](#project-findings)


### Problem Statement:
This project aims to explore the role of classifiers in Hierarchical Temporal Memory (HTM) systems,
focusing on their ability to associate input patterns with meaningful predictions and reconstruct
original inputs from Sparse Distributed Representations (SDRs). By investigating and comparing two
existing classifiers, Htm and KNN, the project seeks to evaluate their functionality,
performance, and differences. Inspired by the SpatialLearning experiment, a new experiment will be
implemented to regenerate input images from SDRs produced by the Spatial Pooler (SP), leveraging
the IClassifier interface for learning and prediction. The experiment will use the ImageEncoder to
process images, reconstruct inputs via classifiers, and compare them with the originals using
similarity measures. Results will be illustrated with diagrams, analysed quantitatively, and discussed,
providing insights into the reconstruction capabilities of classifiers.

### Introduction:
This project explores the integration and application of classifiers within the Hierarchical Temporal Memory (HTM) framework to regenerate input data from Sparse Distributed Representations (SDRs). The core goal is to understand the role of classifiers in reverse encoding, where the learned SDR representations are used to reconstruct the original input. Through this process, we aim to analyse the behaviour and performance of two existing classifiers— Htm and KNN to implement a new experiment that leverages their capabilities. The two classifiers under study in this project—HTMClassifier and KNN—serve as foundational implementations. The HTMClassifier leverages the principles of temporal memory within HTM, while the KNN classifier employs a distance-based approach to classify SDRs based on nearest neighbours. Through this investigation, the project bridges the gap between abstract HTM theories and practical applications, contributing to advancements in intelligent systems and neural computation.

### The Overview of the Project:
![Copy of Copy of Untitled presentation](https://github.com/user-attachments/assets/224f852d-f240-4f4b-8954-3b9e31eec1ed)

### Image Encoder:
The ImageEncoder plays a crucial role in preparing image data for processing within Hierarchical
Temporal Memory (HTM) systems by converting raw image inputs into binary representations
compatible with HTM's Sparse Distributed Representations (SDRs). Based on the ImageBinarizer
NuGet package, the ImageEncoder encodes pixel intensity or feature information into a format that
preserves essential patterns while reducing redundancy. This encoding ensures that similar images
produce similar SDRs, a key characteristic that enables effective learning and pattern recognition in
HTM systems. By preprocessing images into this sparse binary format, the ImageEncoder bridges the
gap between raw image data and the HTM's Spatial Pooler, making it a foundational component for
image-based experiments, such as learning spatial patterns or regenerating inputs from SDRs. https://www.nuget.org/packages/ImageBinarizer

### Encoding Process:
- **Image Binarization** : Input images are binarized using thresholds for RGB channels and resized. Binarized images are saved for further processing.
- **Binary Conversion** : Binarized images are converted into a binary array (inputVector) for processing with the NeoCortex framework.
- **Spatial Pooling** : The binary input vector is processed using the Spatial Pooler to generate Sparse Distributed Representations (SDRs), identifying active columns.
- **Labeling and Storage** : SDRs are labeled with image names and stored for training and prediction in downstream tasks like classification.
- **Visualization** : Similarity plots are generated for analysis of the encoding process.

### Sparse Distributed Representations (SDR):

Sparse Distributed Representations (SDRs) are analogous to how the human brain encodes
information. Just as neurons in the brain fire in sparse patterns, with only a small fraction of neurons
active at any time, SDRs use binary vectors where a small percentage of bits are active (1s) while the
rest are inactive (0s). In the brain, these sparse activations ensure energy efficiency and robustness,
as the overlap in neural firing patterns helps identify similar stimuli. Similarly, SDRs are sparse to
reduce computational complexity and distributed to make the system resilient to noise or
corruption. Technically, SDRs preserve key properties of input data, such as similarity and
distinctiveness, through overlapping active bits for similar inputs and distinct patterns for dissimilar
inputs. This allows HTM systems to efficiently encode, recognize, and generalize patterns, just as the
brain does when processing sensory input. SDRs form the foundation for all processing stages in
HTM, including spatial pooling and temporal memory, providing a biologically plausible and
computationally robust framework for learning and prediction.https://www.frontiersin.org/journals/computational-neuroscience/articles/10.3389/fncom.2017.00111/full

### Hierarchical Temporal Memory (HTM)

Hierarchical Temporal Memory is a theoretical framework and machine learning approach inspired by the structure and function of the neocortex in the human brain. Temporal Memory (TM) is a key component of HTM. It is the mechanism responsible for learning and predicting sequences of patterns based on temporal context. It is designed to capture and store the sequence in which events occur, allowing the system to recognize and predict temporal patterns. The workflow of Hierarchical Temporal Memory (HTM) involves processing input data into sparse distributed representations (SDRs) using an encoder, which captures the essential features of the input. These SDRs are passed to the spatial pooler, which ensures sparsity and generalizes similar inputs. The temporal memory then learns and stores temporal sequences by activating specific cells in its structure based on previous contexts, forming associations over time. As new inputs arrive, the temporal memory predicts future sequences by activating likely next-step cells based on learned patterns.https://www.numenta.com/blog/2019/10/24/machine-learning-guide-to-htm/ 

### Spatial Pooler (SP):

The Spatial Pooler (SP) is a core element of Hierarchical Temporal Memory (HTM) systems, converting raw input data into Sparse Distributed Representations (SDRs). It ensures key properties like sparsity, which improves efficiency and reduces noise sensitivity, and similarity preservation, where similar inputs produce overlapping SDRs for effective pattern recognition. Through competition among columns of cells, guided by adaptive synaptic connections, the SP learns the statistical structure of the input space, making it robust to noise and capable of generalization. This forms the foundation for advanced processes like temporal learning and classification.

### Spatial Pooler Functions

- Converts binarized image inputs into a high-dimensional numerical format.
- Activates a small set of columns to efficiently use memory and recognize patterns.
- Strengthens connections to frequently active input bits for stable pattern recognition.
- Ensures similar inputs generate similar SDRs while handling noise and minor variations.
- Maintains network stability and prevents excessive changes.
- Uses learned SDRs for classification and prediction tasks. https://www.frontiersin.org/journals/computational-neuroscience/articles/10.3389/fncom.2017.00111/full

### Phases of the Spatial Pooler:

**Overlap Phase:** Each column evaluates how well it matches the input by counting active synapses.
**Inhibition Phase:** The best-matching columns stay active while others are suppressed.
**Learning Phase:** Connections are adjusted based on experience—correct predictions are reinforced, and incorrect ones are weakened. This process ensures stable, sparse, and efficient pattern recognition. https://www.frontiersin.org/articles/10.3389/fncom.2017.00111/full

### HTM Classifiers:
Hierarchical Temporal Memory (HTM) classifiers are essential for sequence learning and pattern recognition within the HTM framework. They link Sparse Distributed Representations (SDRs) from the Spatial Pooler and Temporal Memory to meaningful labels. During training, the classifier maps SDRs to output labels and, once trained, can predict the most likely label for new or partial inputs. Unlike traditional classifiers, HTM classifiers are biologically inspired, allowing them to handle noise, detect anomalies, and generalize from sparse inputs. Their incremental learning capability makes them ideal for real-time tasks like image recognition and time-series forecasting.

**Methodology:** The process involves data preprocessing to convert inputs into SDRs, stable encoding by the Spatial Pooler, and classifier training to associate SDRs with labels. Upon receiving new input, the classifier matches it with stored SDRs and assigns the most probable label. Performance evaluation and optimization, such as hyperparameter tuning, further enhance accuracy. https://numenta.com/neuroscience-research/

### KNN Classifiers:
The KNN classifier is a simple, non-parametric algorithm that classifies new data points based on their proximity to labeled training samples. When provided with an SDR or a feature vector, KNN calculates the distance to the "k" nearest neighbors and assigns the most frequent label. In this project, KNN can serve as a baseline classifier to compare HTM's generalization capabilities with KNN’s similarity-based approach. However, KNN lacks the adaptive learning ability of HTM, making it less effective for evolving data streams.

**Methodology:** KNN’s learning phase involves storing labeled SDR sequences, avoiding duplicates, and managing a limited storage capacity to maintain efficiency. During classification, it computes the distance between an unknown input and stored SDRs, ranking results by similarity. A voting mechanism then assigns the most common label among the nearest neighbors. The model adapts over time, dynamically updating stored sequences for better pattern recognition. https://scikit-learn.org/stable/ 

### Difference between HTM and KNN Classifiers:
The primary distinction between Hierarchical Temporal Memory (HTM) and K-Nearest Neighbors (KNN) lies in their learning approach, adaptability, and handling of high-dimensional Sparse Distributed Representations (SDRs).

- **Learning Paradigm:** HTM is a biologically inspired model that learns spatial and temporal patterns dynamically through unsupervised learning, while KNN is a non-parametric, supervised algorithm that relies on labeled data for classification without adapting to new data over time.
- **Adaptability:** HTM continuously updates its learning as new data arrives, making it ideal for evolving data streams. In contrast, KNN remains static unless manually updated.
- **Handling High-Dimensional Data:** HTM effectively processes sparse, high-dimensional SDRs, while KNN struggles with the "curse of dimensionality," leading to reduced efficiency and accuracy.
- **Pattern Reconstruction:** HTM's spatial pooler can reconstruct input patterns using learned synaptic connections, while KNN lacks this capability, focusing solely on classification.
- **Similarity Metrics:** HTM leverages metrics like the Jaccard Index for evaluating overlap between original and reconstructed SDRs, while KNN only provides insights into neighbor-based influence. https://www.numenta.com/

### Reconstruction of Images:
The reconstruction process regenerates an image representation from its Sparse Distributed Representation (SDR). Each binarized input image is processed by the Spatial Pooler (SP), which identifies active columns representing key features. Using these active columns, the SP reconstructs permanence values, estimating the likelihood of each pixel being active. Pixels that were not part of the active columns are assigned a permanence value of zero to maintain consistency. The permanence values are then sorted and normalized using a thresholding method, converting them into a binary-like format. Values above a set threshold (e.g., 40.5%) are marked as 1 (active), while lower values are 0 (inactive). The reconstructed image is then saved in text format, with separate folders (ReconstructedHTM and ReconstructedKNN) for images reconstructed using the HTM and KNN classifiers, allowing comparative evaluation.

### Methodology
 **There are various Methods used in the Experiment keeping in mind the concept of code reusabilty:**
- **1. Program.cs** -This C# code sample demonstrates a basic experiment framework for implementing the Spatial Pooler (SP) algorithms using the NeoCortexApi library. The program presents a console-based menu allowing users to run Spatial Pooler and thereby predicting images with two classifiers namely HTM and KNN.

- **2. ImageBinarizerSpatialPattern.cs** - This C# code showcases the functionality of Spatial Pooler, Classifier Training and Prediction, Reconstruction and Similarity comparsions. The set of images are divided into Training Dataset (80%) and Testing Dataset(20%) then both the set of images undergo binarization, the binarized images are converted into a one-dimensional array  which is further transformed into a list. This list is fed into the Spatial Pooler for training and SDR generation. The training process persists until the spatial pooler achieved stability, with oversight from the HomeostaticPlasticityController (HPC) class. The generated SDRs are passed to both the classifier in order to train them with the test images. After the completion of the training phase, the classifier starts it's prediction phase to get the best predicted images based on the test dataset. As this experiment ends the classifiers are resetted for the next experiment. Thereafter the predicted images by both the classiffiers are reconstructed to binarized form and the similarity between the original images and reconstructed images is calculated. With the similarities obtained from both the classifiers, a Scott Plot and a Graph Plot is constructed. The classifier performance is also calculated based to the similarities obtained.

- **3. BinarizerImage.cs** - This C# code processes an input image by converting it into a binary text representation based on predefined color thresholds and saves the result as a text file. It first constructs the output file path by appending .txt to the specified destinationPath and removes any existing file with the same name to ensure a fresh output. The function then creates an instance of ImageBinarizer with parameters that define the binarization process, including red, green, and blue threshold values set at 200, the desired output image dimensions, and the input and output file paths. The Run method is executed to process the image, converting pixels that exceed the RGB thresholds into active ('1') values while marking others as inactive ('0'). Finally, the function returns the path to the generated text file, which contains a structured binary representation of the original image that can be used further for Spatial Pooler training.

- **4. ImageReconstruction.cs** - The ImageReconstructor.cs class reconstructs images from Sparse Distributed Representations (SDRs) using predicted active columns from classifiers. It extracts active columns, estimates permanence values with the Spatial Pooler (SP), normalizes them into a binary format, and saves the output as a text file. Inactive columns are assigned a permanence value of 0.0 to maintain consistency. The reconstructed images are stored in separate folders (ReconstructedHTM and ReconstructedKNN) for performance comparison between the HTM and KNN classifiers. Also the Similarity between the original and reconstructed image is calculated here by JaccardSimilarityofBinaryArrays. This process visualizes how accurately the classifiers predict original images based on learned SDR patterns.

- **5. htmClassifier.cs/knnClassifer.cs/IClassifier.cs** - These classes implements the classifiers, htmClassifier and knnClassifier, for image pattern recognition using Sparse Distributed Representations (SDRs) from the NeoCortex API. Both classifiers adhere to the IClassifier interface, which defines methods for learning input patterns, predicting future inputs, clearing internal state, and reconstructing inputs from predictions.

### Project Findings
This project aims to deploy the Reconstruct and Similarity functionalities between the images and classifiers.

- The resultant Comparisons of similarities(Jaccard Index) of the Original Binarized image and the reconstructed image for both the classifier. The result is also plotted with a Scott Plot and Bar Graph for visualisation.
- The resultant Comparion of performance of both the Classifier to understand the which classifer performs better.  
