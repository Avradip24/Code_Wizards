# Getting Started
### Instructions to Run the project:

- Clone the repository from [Code_Wizards](https://github.com/Avradip24/Code_Wizards)
- Checkout to the master branch
- The main project files can be found in [Project Files](https://github.com/Avradip24/Code_Wizards/tree/master/source/Samples/NeoCortexApiSample)
- Run the "dotnet restore" command in command prompt.
- The project requires input images to perform the experiment. Few input images should be placed inside the file - "Code_Wizards\source\Samples\NeoCortexApiSample\bin\Debug\net9.0\TestFiles". You can find the input images we used inside [TestFiles](https://github.com/Avradip24/Code_Wizards/tree/master/source/Samples/NeoCortexApiSample/TestFiles)
- Set the starting point of the project as NeoCortexApiSample
  
![Project Starting Point](https://github.com/user-attachments/assets/38dcab40-c09a-4703-b543-d4e6130c617e)
|:--:| 
| Fig 1: Set StartUp Project |
- Run the Experiment
- The project starts from the Program.cs file [Program.cs](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/Program.cs)
- The classifiers performance is displayed in the Console and the visualizations are saved in the output folders mentioned below
- The Unit Test files are available inside [Unit Tests](https://github.com/Avradip24/Code_Wizards/tree/master/source/TestNeoCortexApiSample)

### The outputs will be saved here:

- Code_Wizards\source\Samples\NeoCortexApiSample\bin\Debug\net9.0\BinarizedImages
- Code_Wizards\source\Samples\NeoCortexApiSample\bin\Debug\net9.0\HTMSimilarityPlot
- Code_Wizards\source\Samples\NeoCortexApiSample\bin\Debug\net9.0\KNNSimilarityPlot
- Code_Wizards\source\Samples\NeoCortexApiSample\bin\Debug\net9.0\ReconstructedHTM
- Code_Wizards\source\Samples\NeoCortexApiSample\bin\Debug\net9.0\ReconstructedKNN

### Project Directory Structure:
On cloning the repository, the below directory structure is followed : 
![Project Directory](https://github.com/user-attachments/assets/9be20ccf-d9fb-4b23-9f3d-1feef3f441f4)
|:--:| 
| Fig 2: Project Directory Structure |

### Unit Tests Directory Structure:
This is the directory structure for Unit Tests :
![Directory 2](https://github.com/user-attachments/assets/b5ce3034-f655-4df6-a7a6-ddffebc93cb8)
|:--:| 
| Fig 3: Unit Tests Directory Structure |
