# git-memory
Capture snapshots of your work as memorable milestones by saving selected commits from Git.

## Objective:
### The Problem:
As developers grow familiar with Git, there’s often a need to revisit old solutions for reuse in new projects. Different approaches have emerged to address this, such as:
- Saving as separate projects in the cloud and downloading them when needed;
- Copying the full project to another location and later struggling to find which files were changed to solve a problem;
- Using Gists, which are limited to single files and algorithms, making it difficult to track the full history of a functionality;
- Advanced Git users create custom commands to store files from a commit as a zip file in a specific location.

### Conclusion: There's No Standard Way to Save Functionality Snapshots
The objective of this project is to provide a **standardized way** to store code snapshots, or "memories," by grouping commits that are related to a particular functionality within a `git-memory` repository structure.

## How It Works:
Using the command line, similar to Git, you can perform the following operations:
- Set a folder as the location to store your "memories" (let's call it `Memories`).
- When working on a specific project, easily select commits related to a particular functionality.
- Run a command like `gitm save "New Functionality"`, which will create a folder inside `Memories` containing the content of the selected commits.
- A new Git repository will be initialized inside `Memories/New Functionality`, capturing all the files involved in the commits.
- The folder will also include an `index.html` file to navigate through the commit changes and visualize the step-by-step creation of the functionality, in the original order.

## Features:
- Standardized structure for storing Git "memories" (snapshots of your commits).
- Easy selection of related commits to capture a specific functionality.
- Automatically generates a new Git repository for each functionality.
- Visual commit history accessible through a generated HTML file.

## Technologies:
This project is written in **C#**, leveraging the `libgit2sharp` library to manipulate Git repositories programmatically.
