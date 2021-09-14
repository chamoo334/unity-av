# unity-av

### Todo

- [ ] Menu Screen

  - [ ] Options Menu Scene
    - [ ] User option to select 3 visuals
      - [ ] one center (atoms or circles)
      - [ ] one block based (Vshape, front, back)
      - [ ] one spacial (fireflies minimal, blender effect, )
      - [ ] set standard to V-shape, atoms, back cubes

- [ ] Game Play

  - [ ] attach graphics as defined in user options
  - [ ] Pause Menu
    - [ ] Exit/Back buttons and functionality
      - [ ] clear songs and graphics on execution
    - [ ] Adjust music volume
    - [ ] Change song

- [ ] Smooth scene loading transitions

- Version 2:

  - [ ] Optimize with gpu

- Extras:
  - [ ] Check FFT absolute values to always render above zero or manually shift in script.
  - [ ] Check bitdepth for audio calculations
  - [ ] Lighting
  - [ ] Filters for low, mid, high and optional object attachments

### In Progress

- [ ] Graphics
  - [ ] fireflies (minimal)
    - [ ] set particle system to BPM
  - [ ] Blender particle system
  - [ ] circles
  - [ ] 512 circled cubes -> convert to fireflies
  - [ ] center piece (skull or circles)
  - [ ] fireworks
  - [ ] fractals

### Done ✓

- [ ] Menu Screen

  - [x] Add quiet music
  - [x] Main Menu Scene
    - [x] Play button with correct functionality
    - [x] Options button with correct functionality
    - [x] Quit button with correct functionality
  - [ ] Options Menu Scene
    - [x] Back button with functionality
    - [x] Volume slider with functionality
    - [x] Song selection

- [ ] Game Play

  - [x] Box to contain visuals
  - [x] Option to change view to:
    - [x] top
    - [x] bottom
    - [x] front
  - [x] Attach song
  - [x] Song data processing
    - [x] Separate into 8 bands
  - [ ] Graphics
    - [x] 16 cubes in V-shape
    - [x] 8 cubes centered
    - [x] 8 cubes in back
    - [x] 8 cubes in front
    - [x] atoms
