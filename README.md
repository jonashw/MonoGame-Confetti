# MonoGame Confetti

This is a simple particle system inspired by the celebratory confetti in [Threes](http://asherv.com/threes/).

There are 2 emission styles contained here, though additional styles could be added:

- Sprinkle
![Sprinkle](/demo-animations/sprinkle.gif)

- Explode
![Explode](/demo-animations/explode.gif)

## Sample usage

- [Open Solitaire Classic](https://github.com/SoundGoddess/OpenSolitaire)
![Open Solitaire Classic](/demo-animations/in-open-solitaire-classic.gif)

## Important note:
There has been no effort to optimize the performance of this particle system;
particles are created but never released from memory (even when they exit the screen).
It might be worthwhile to
[check for memory leaks with a profiler](http://stackoverflow.com/questions/13473761/perfmon-counters-to-check-memory-leak)
and then perhaps [implement an Object Pool](http://gameprogrammingpatterns.com/object-pool.html) to conserve resources.
