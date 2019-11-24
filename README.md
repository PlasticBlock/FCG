# Random Walk Caves Generator

RWCG (Random Walk Caves generator) is procedural 2D caves generation system written for Unity.

## Install Package

Git upm package link
https://github.com/vmp1r3/RandomWalkCavesGenerator.git#upm

## Cave generation process

### Cave generation in Matrix object.

Cave's data is generating inside seperate `Matrix` object according to size property and pseudorandom algorithm's result.
On this stage, matrix is filling recursive by walking to different random directions.
Each seed returns same results so if you want to save your result just save seed that you've used to generate cave.

### Mesh generation
After matrix is generated, `EnvironmentGenerator` component, generates separete matrices by slicing main matrix into
separete chunks and provides them to the `MeshGenerator` component which generates Unity's Meshes.

<p>
	<img alt="cave 1" src="http://plasticblock.xyz/projects/rwcg/cave-example-1.png" width="240">
	<img alt="cave 2" src="http://plasticblock.xyz/projects/rwcg/cave-example-2.png" width="240">
</p>
Normals and separate chunks.
<p>
	<img alt="normals" src="http://plasticblock.xyz/projects/rwcg/cave-mesh-structure-1.png" height="240">
	<img alt="chunks" src="http://plasticblock.xyz/projects/rwcg/cave-mesh-structure-2.png" height="240">
</p>


## CONTACTS
<br>*Website* http://plasticblock.xyz
<br>*E-mail* contact@plasticblock.xyz.

## LICENSE
<br>Licensed under GPLv3 license or under special license. See the LICENSE file in the project root for more information.