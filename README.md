# Flocking

## Instructions
Click on the screen to move the flock

## Implemented
- Represent a boid. Visually show orientation.
- Make obstacles
- Make a Flock, a container of boids. Generate steering impulses here (or pass
location of other flock-mates to the boid). The behavior:
  - Don’t allow the boid to move into an obstacle (raw adjustment of position)
  - Move towards mouse if the user is clicking (but still allow some movement for the
cohesion, seperation and alignment impulses)
  - If the user is not clicking, do a combination of separation, cohesion and alignment.
- Cone-based vision detection algorithm using dot-product
- Make the boid actively avoid steering into an obstacle.
