<h1 align="center">🏔 Caves Generator 🏔</h1>

Random 2D caves generation algorithm example.

### Use
Add `CavesGenerator` component to empty gameObject  
Set desirable size, scale, boundary, amout of divisions and empty cells count  
Generator is pseudorandom, in case when you want to get the same result. 
To do that, turn autoseed off and set desired seed.

### Algorithm
- Cave is a 2D sheet where each cell has rule eather it is filled or not.
- We start from cell at the middle/center.
- Each cell we enter becomes empty.
- Then next cell is selected from neigbouring cells randomly, from up, down, left and right.
- Iteration repeat until desiarable count of cells are empty

<p>
<img src="https://user-images.githubusercontent.com/14846427/124689440-36016100-dee1-11eb-93eb-0a51eec09bd9.png" height=240 />
<img src="https://user-images.githubusercontent.com/14846427/124689438-3568ca80-dee1-11eb-8232-6418d6cdd7a7.png" height=240 />
</p>
