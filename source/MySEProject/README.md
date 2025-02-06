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
- [K-Nearest Neighbours (KNN) Classifiers](#k-nearest-neighbours-knn-classifiers)
  - [Methodolgy of KNN Classifier](#methodolgy-of-knn-classifier)
- [HTM Classifiers](#htm-classifiers)
  - [Methodolgy of HTM Classifier](#methodolgy-of-htm-classifier)
- [Difference between HTM and KNN Classifiers](#difference-between-htm-and-knn-classifiers)
- [Methodology](#methodology)


### Problem Statement:
This project aims to explore the role of classifiers in Hierarchical Temporal Memory (HTM) systems,
focusing on their ability to associate input patterns with meaningful predictions and reconstruct
original inputs from Sparse Distributed Representations (SDRs). By investigating and comparing two
existing classifiers, HtmClassifier and KNN, the project seeks to evaluate their functionality,
performance, and differences. Inspired by the SpatialLearning experiment, a new experiment will be
implemented to regenerate input images from SDRs produced by the Spatial Pooler (SP), leveraging
the IClassifier interface for learning and prediction. The experiment will use the ImageEncoder to
process images, reconstruct inputs via classifiers, and compare them with the originals using
similarity measures. Results will be illustrated with diagrams, analysed quantitatively, and discussed,
providing insights into the reconstruction capabilities of classifiers in HTM systems and their
practical implications.

### Introduction:
This project explores the integration and application of classifiers within the Hierarchical Temporal Memory (HTM) framework to regenerate input data from Sparse Distributed Representations (SDRs). The core goal is to understand the role of classifiers in reverse encoding, where the learned SDR representations are used to reconstruct the original input. Through this process, we aim to analyse the behaviour and performance of two existing classifiers—HtmClassifier and KNN—and implement a new experiment that leverages their capabilities. The two classifiers under study in this project—HtmClassifier and KNN—serve as foundational implementations. The HtmClassifier leverages the principles of temporal memory within HTM, while the KNN classifier employs a distance-based approach to classify SDRs based on nearest neighbours. Through this investigation, the project bridges the gap between abstract HTM theories and practical applications, contributing to advancements in intelligent systems and neural computation.

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
image-based experiments, such as learning spatial patterns or regenerating inputs from SDRs.

### Encoding Process:
- **Image Binarization** : Input images are binarized using thresholds for RGB channels and resized. Binarized images are saved for further processing.
  
- **Binary Conversion** : Binarized images are converted into a binary array (inputVector) for processing with the NeoCortex framework.
  
- **Spatial Pooling** : The binary input vector is processed using the Spatial Pooler to generate Sparse Distributed Representations (SDRs), identifying active columns.
  
- **Labeling and Storage** : SDRs are labeled with image names and stored for training and prediction in downstream tasks like classification.
  
- **Visualization** : 1D heatmaps and similarity plots are generated for analysis and monitoring of the encoding process.

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

Hierarchical Temporal Memory is a theoretical framework and machine learning approach inspired by the structure and function of the neocortex in the human brain. Temporal Memory (TM) is a key component of HTM. It is the mechanism responsible for learning and predicting sequences of patterns based on temporal context. It is designed to capture and store the sequence in which events occur, allowing the system to recognize and predict temporal patterns. The workflow of Hierarchical Temporal Memory (HTM) involves processing input data into sparse distributed representations (SDRs) using an encoder, which captures the essential features of the input. These SDRs are passed to the spatial pooler, which ensures sparsity and generalizes similar inputs. The temporal memory then learns and stores temporal sequences by activating specific cells in its structure based on previous contexts, forming associations over time. As new inputs arrive, the temporal memory predicts future sequences by activating likely next-step cells based on learned patterns. 

### Spatial Pooler (SP):

The Spatial Pooler is a fundamental component of Hierarchical Temporal Memory (HTM) systems, transforming raw input data into Sparse Distributed Representations (SDRs). Its primary function is to encode the input while ensuring key properties such as sparsity and similarity preservation. Sparsity ensures that only a small percentage of bits in the SDR are active, which improves computational efficiency and reduces noise sensitivity. Similarity preservation means that inputs with similar patterns produce SDRs with overlapping active bits, enabling the system to recognize related patterns effectively. The Spatial Pooler achieves this through competition among columns of cells, where each column competes to represent specific input features, guided by synaptic connections that adapt over time. This adaptation allows the Spatial Pooler to learn the statistical structure of the input space, making it robust to noise and capable of generalizing from limited data. As a result, the Spatial Pooler provides the foundation for further processing, such as temporal learning and classification, in HTM systems.

### Spatial Pooler Functions 
Here’s a simplified breakdown of Spatial Pooler functions:

**Input Binarization & Encoding**: The SP receives binarized image inputs and encodes them into a high-dimensional numerical format.
**Sparse Distributed Representations (SDRs)**: The SP converts dense input patterns into SDRs by activating a small set of columns, ensuring efficient memory usage and robust pattern recognition.
**Learning & Adaptation**: Over multiple training cycles, the SP strengthens connections to frequently active input bits, reinforcing stable pattern recognition.
**Stability & Generalization**: The SP ensures that similar inputs generate similar SDRs while also achieving invariance to minor variations, making the model resilient to noise and distortions.
**Homeostatic Plasticity Control**: It monitors and maintains network stability, ensuring that the system adapts effectively while preventing excessive changes.
**Classifier Integration**: The learned SDRs are later used for classification tasks, enabling recognition and prediction of input patterns based on prior learning.
https://www.frontiersin.org/journals/computational-neuroscience/articles/10.3389/fncom.2017.00111/full


### Phases of the Spatial Pooler:

The Spatial Pooler (SP) works in three steps: overlap, inhibition, and learning. First, in the overlap phase, each column in the SP checks how well it matches the input by counting the number of strong connections (synapses) receiving active signals ("1"). The more strong connections it has, the better it matches the input. Next, in the inhibition phase, only the best-matching columns stay active, while others are suppressed. This competition ensures that only the most relevant patterns are represented. Finally, in the learning phase, the SP adjusts its connections based on experience—correct predictions strengthen connections, while incorrect ones weaken them. Over time, this allows the SP to recognize patterns consistently, even with slight changes or noise in the input. This process ensures stable, sparse, and efficient pattern recognition.

Reference: Cui, Y., Ahmad, S., & Hawkins, J. (2017). The HTM Spatial Pooler—A Neocortical Algorithm for Online Sparse Distributed Coding. Frontiers in Computational Neuroscience. [https://www.frontiersin.org/articles/10.3389/fncom.2017.00111/full](https://www.frontiersin.org/journals/computational-neuroscience/articles/10.3389/fncom.2017.00111/full)

### HTM Classifiers:
Hierarchical Temporal Memory (HTM) classifiers play a crucial role in sequence learning and pattern recognition within the HTM framework. They are designed to associate Sparse Distributed Representations (SDRs) generated by the Spatial Pooler and Temporal Memory with meaningful labels, allowing the system to make predictions based on learned patterns. The HTM classifier works by mapping input SDRs to their corresponding output labels during the learning phase. Once trained, it can retrieve the most likely labels when presented with new or partial input patterns, making it particularly effective for recognizing temporal sequences and structured data. Unlike traditional classifiers, HTM classifiers leverage the biological principles of the neocortex, enabling them to handle noise, detect anomalies, and generalize from sparse inputs. Their ability to incrementally learn without requiring retraining makes them well-suited for real-time applications, such as image recognition, anomaly detection, and time-series forecasting. By continuously refining associations between SDRs and labels, HTM classifiers contribute to the adaptability and robustness of HTM-based systems.

### Methodolgy of HTM Classifier:

- **Data Preprocessing:** Input data (e.g., images) is binarized and converted into Sparse Distributed Representations (SDRs) for HTM processing.
  
- **Spatial Pooling & Encoding:** The Spatial Pooler (SP) generates stable SDRs, capturing essential patterns from the input data.
  
- **Training the Classifier:** The classifier learns associations between SDRs and class labels, updating dynamically as new data is processed.
  
- **Classification & Prediction:** When a new input is received, it is matched against stored SDRs, and the most probable label is assigned.
  
- **Evaluation & Optimization:** Performance is measured using similarity metrics, and enhancements like hyperparameter tuning or additional classifiers can improve accuracy. https://numenta.com/neuroscience-research/

### K-Nearest Neighbours (KNN) Classifiers:
The K-Nearest Neighbours (KNN) classifier is a simple, non-parametric algorithm used for classification and regression tasks. It stores all the labeled training data and classifies new data points based on their similarity to the closest training samples. When a new input is provided, the algorithm computes its distance (commonly using Euclidean distance) from all training points, identifies the "k" nearest neighbours, and assigns the most common label among those neighbours to the input. In the context of this project, KNN could serve as a baseline classifier or a comparative model for evaluating SDR representations. When provided with an SDR or a derived feature vector, KNN computes distances (e.g., Euclidean) to its "k" closest neighbours and predicts the most frequent label among them. This method can be useful in this project to classify SDRs or reconstructed patterns, allowing comparisons between HTM's ability to generalize patterns and KNN's reliance on proximity and similarity. While KNN is straightforward and effective for small-scale problems, it lacks the adaptive learning and biological inspiration of HTM, making it less dynamic for processing evolving data streams.scikit-learn.org

### Methodolgy of KNN Classifier:
- **Learning Phase of the KNN Classifier:** The learning phase of the KNN classifer involves storing and managing labeled Sparse Distributed Representations (SDRs) to facilitate classification. Below are the key points outlining how learning is implemented:

- **Training Data Storage:** The classifier maintains a dictionary _sdrMap, which maps each label (e.g., "A", "B", "C") to a list of SDR sequences.
Each SDR sequence represents the active cells of an input pattern. Learn(TIN input, Cell[] cells) method is responsible for adding new SDR sequences to the model. It extracts the active cell indices from the cells array and converts them into an integer array.

- **Avoiding Duplicate Sequences:** Before adding a new SDR sequence, the classifier checks if the exact sequence already exists for a given label.
If an identical sequence is found, it is ignored to prevent redundancy.

- **Maintaining Limited Storage Capacity:** The classifier keeps a maximum of _sdrs (default: 10) SDR sequences per label.
If the number of stored sequences exceeds this limit, the oldest sequence is removed (FIFO mechanism).

- **Incremental Learning:** The classifier dynamically updates the stored sequences, allowing it to adapt to new patterns over time.
This makes it suitable for handling changes in input distributions while preserving previously learned patterns. This learning process ensures that the classifier builds a reference dataset of labeled SDRs, which will later be used in the classification (prediction) phase to determine the closest match for unknown inputs.

- **Similarity Checking:** The similarity checking phase of the KNN (K-Nearest Neighbors) classifier is a critical step where the model evaluates how closely an unknown input resembles previously classified data. This process involves computing the distance between the unclassified input and stored labeled sequences using distance metrics such as Euclidean distance, Manhattan distance, or Hamming distance, depending on the dataset’s nature. The classifier then ranks the results based on the lowest computed distances, with the closest matches assigned higher similarity scores. A voting mechanism is used where the most frequently occurring labels among the nearest neighbors determine the classification of the unknown input. If the overlap between the unknown and classified sequences exceeds 50%, direct similarity is prioritized; otherwise, the model follows a majority voting approach.KNN’s effectiveness depends on the choice of distance metric and the number of neighbors considered, which directly influence classification accuracy. https://scikit-learn.org/stable/ 

### Difference between HTM and KNN Classifiers:
The core difference between Hierarchical Temporal Memory (HTM) and K-Nearest Neighbors (KNN) lies in their approach to learning, adaptability, and the ability to handle high-dimensional data like Sparse Distributed Representations (SDRs). When analyzed in the context of this project, these differences highlight the strengths and weaknesses of each model in processing binarized image data and reconstructing patterns.

- **Learning Paradigm:** HTM is a biologically inspired model designed to mimic how the human brain processes spatial and temporal data. It adapts to input patterns dynamically, learning spatial relationships in the input data over time through unsupervised mechanisms. In this project, HTM's spatial pooler captures the structure of SDRs from binarized images and reconstructs them efficiently, showing its capacity to learn and generalize patterns dynamically. In contrast, KNN is a non-parametric supervised learning algorithm that relies on labeled data for training. KNN does not "learn" patterns or adapt to new data over time; it simply compares new input data to its neighbors in the feature space, making it unsuitable for tasks requiring pattern reconstruction or adaptation without explicit labels.

- **Adaptability:** HTM excels in its ability to continuously adapt to new inputs while preserving previously learned patterns. This makes it ideal for the iterative process of improving pattern recognition and reconstruction in this project. On the other hand, KNN does not adapt to new data unless the dataset is explicitly updated. This static nature makes it less suitable for evolving or streaming data scenarios, which are central to HTM's design.

- **Handling High-Dimensional Data:** The project deals with binary SDRs, which are high-dimensional and sparse by nature. HTM's design explicitly handles such data effectively by focusing on sparse, distributed representations. KNN, on the other hand, suffers from the "curse of dimensionality," where the distance metrics used for classification become less meaningful as dimensionality increases. This makes KNN less efficient and accurate when dealing with SDRs compared to HTM.

- **Reconstruction of Input Patterns:** HTM's spatial pooler not only classifies input patterns but also reconstructs them, a key requirement in this project. It achieves this by leveraging the learned synaptic connections and activation patterns. KNN lacks any mechanism for reconstructing input patterns since it is purely a classification algorithm, relying only on proximity-based voting.

- **Similarity Metrics and Interpretability:** In this project, HTM employs similarity metrics like the Jaccard Index to evaluate the overlap between the original and reconstructed SDRs. This interpretability and quantitative measure of learning are inherent to HTM. In contrast, KNN's interpretability is limited to understanding which neighbors influence a classification decision, and it does not contribute to understanding the underlying structure of the data.

### Methodology
 **There are various Methods used in the Experiment keeping in mind the concept of code reusabilty:**
- **1. Program.cs** -This C# code sample demonstrates a basic experiment framework for implementing the Spatial Pooler (SP) algorithms using the NeoCortexApi library. The program presents a console-based menu allowing users to run Spatial Pooler and thereby predicting images with two classifiers namely HTM and KNN.

- **2. ImageBinarizerSpatialPattern.cs** - This C# code showcases an experiment focusing on spatial pattern learning using the NeoCortex API. It utilizes the Hierarchical Temporal Memory (HTM) model, particularly the Spatial Pooler (SP) algorithm, to learn images from input sample images presented as binarized values. Then the SDRs that were generated by the Spatial Pooler were passed to both the classifiers in order to train them with the images. As training phase gets completed the Classifiers start it's prediction phase to get top 3 predicted images. As this experiment ends the classifiers are reseted for the next experiment.

- **3. ImageBinarizer.cs** - This C# code processes an input image by converting it into a binary text representation based on predefined color thresholds and saves the result as a text file. It first constructs the output file path by appending .txt to the specified destinationPath and removes any existing file with the same name to ensure a fresh output. The function then creates an instance of ImageBinarizer with parameters that define the binarization process, including red, green, and blue threshold values set at 200, the desired output image dimensions, and the input and output file paths. The Run method is executed to process the image, converting pixels that exceed the RGB thresholds into active ('1') values while marking others as inactive ('0'). Finally, the function returns the path to the generated text file, which contains a structured binary representation of the original image that can be used further for Spatial Pooler training.
