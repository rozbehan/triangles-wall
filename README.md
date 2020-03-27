# Algorithm problem
Input: A wall and a few number of segments (between 5 and 9) with different lengths
Start the game by placing a segment along the wall. Make the triangles by placing the rest of the segments so that the farthest vertex of the triangles gets the maximum possible distance from the wall.

1. I have tried on a binary tree instead of triangle-mesh, based on duality. 
2. Every segment is a node in binary tree and two new vertices of the triangle are the children of the node. 
3. The binary tree has a root, the base segment with two vertices of (0,0), (length,0). 
4. A recursive function has tried to make all the possible trees. 
5. The next triangle will be made only through one of the last two segments. The binary tree will only grow through one of the last two nodes. 
6. After selecting two new segments among the rest segments, we have 8 options to make a new triangle with them. 
(2 base segments) * (swapping 2 new segments) * (2 mirroring). The same is for the binary tree. 
7. We skip the non-triangle set. 
8. We calculate the coordinate of the new vertex by having the three(or two) edges and two vertices. 
9. We keep the maximum of the y-dimension among the coordinates of the new vertices. 
10. We skip the triangles with negative y-dimension. 
