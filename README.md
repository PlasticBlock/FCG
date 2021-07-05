# Caves Generator

This project is an example of simple caves generation algorithm which works in next way:
- Sheet with filled cells is created, each cell has rule eather it is filled or not.
- Cell in the center is selected and set as empty and first iteration starts.
- Randomly next cell is choosen from up, down, left and right.
- Cycle restarts, each cell set to empty is counted and when desiarable count of cells are modified, algorithm stops
 
### Install using Unity Package Manager

git upm link https://github.com/vmp1r3/CavesGenerator.git#upm

## Cave generation process

### Matrix object
Cave's data is generating inside seperate `Matrix` object according to size property and pseudorandom algorithm's result.
On this stage, matrix is filling recursive by walking to different random directions.
Each seed returns same results so if you want to save your result just save seed that you've used to generate cave.

### Transforming to Mesh
After matrix is generated, `EnvironmentGenerator` component, generates separete matrices by slicing main matrix into
separete chunks and provides them to the `MeshGenerator` component which generates Unity's Meshes.

<p>
<img src="https://user-images.githubusercontent.com/14846427/124441867-d42be480-dd84-11eb-94ed-5ab89932a0c3.png" height="240">
<img src="https://user-images.githubusercontent.com/14846427/124441901-df7f1000-dd84-11eb-9115-e1417e9742db.png" height="240">
</p>

Normals and separate chunks.

<p>
<img src="https://user-images.githubusercontent.com/14846427/124441982-f4f43a00-dd84-11eb-905f-23dce1384102.png" height="240">
<img src="https://user-images.githubusercontent.com/14846427/124441988-f58cd080-dd84-11eb-9932-d1d4cde8fd26.png" height="240">
</p>
