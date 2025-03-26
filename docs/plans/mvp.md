# Implementation Plan for Corporate Nightmare Game MVP

## Project Setup and Core Structure
- [ ] Step 1: Project configuration and structure setup
  - **Task**: Set up the basic MonoGame project structure and configure display settings
  - **Files**:
    - `Game1.cs`: Update window size, title, and basic game loop structure
    - `Content/Content.mgcb`: Configure content pipeline settings
  - **Step Dependencies**: None
  - **Build & Run**: After this step, you should be able to run the game with a configured window size and title.

- [ ] Step 2: Create core game components
  - **Task**: Implement essential game components and managers
  - **Files**:
    - `GameComponents/GameState.cs`: Create game state management (menu, playing, game over)
    - `GameComponents/InputManager.cs`: Handle keyboard and mouse input
    - `GameComponents/ScoreManager.cs`: Track and manage player score
    - `GameComponents/SoundManager.cs`: Handle audio playback
  - **Step Dependencies**: Step 1
  - **Build & Run**: After this step, you should be able to run the game with the core components initialized.

## Core Game Mechanics
- [ ] Step 3: Implement the Snake entity
  - **Task**: Create the snake player entity with movement mechanics and basic rendering
  - **Files**:
    - `Entities/Snake.cs`: Main snake class with segments, movement logic
    - `Entities/SnakeSegment.cs`: Individual segment class
    - `Game1.cs`: Update to use the Snake class
  - **Step Dependencies**: Step 2
  - **Build & Run**: After this step, you should be able to run the game and see the snake moving on screen.

- [ ] Step 4: Implement basic snake growth and boundaries
  - **Task**: Add functionality for the snake to grow and handle boundary collisions
  - **Files**:
    - `Entities/Snake.cs`: Update to include growth and boundary collision logic
    - `Game1.cs`: Update to handle game over on boundary collision
  - **Step Dependencies**: Step 3
  - **Build & Run**: After this step, you should be able to run the game and observe the snake's boundary collision detection.

- [ ] Step 5: Implement self-collision detection
  - **Task**: Create collision detection for the snake with itself
  - **Files**:
    - `Entities/Snake.cs`: Update to include self-collision detection
    - `Game1.cs`: Update to handle game over on self-collision
  - **Step Dependencies**: Step 4
  - **Build & Run**: After this step, you should be able to run the game and test self-collision.

- [ ] Step 6: Implement basic collectible items
  - **Task**: Create the collectible items system (coffee cups)
  - **Files**:
    - `Entities/Collectible.cs`: Base collectible class
    - `Entities/CoffeeCollectible.cs`: Coffee cup collectible
    - `GameComponents/CollectibleManager.cs`: Manage collectible spawning and collection
    - `Game1.cs`: Integrate collectible manager
  - **Step Dependencies**: Step 5
  - **Build & Run**: After this step, you should see collectable items on screen and be able to collect them.

- [ ] Step 7: Implement office supply collectibles
  - **Task**: Add office supply collectibles for variety
  - **Files**:
    - `Entities/OfficeSupplyCollectible.cs`: Office supply collectible implementation
    - `GameComponents/CollectibleManager.cs`: Update to spawn office supplies
    - `Game1.cs`: Update to handle different collectible types
  - **Step Dependencies**: Step 6
  - **Build & Run**: After this step, you should see multiple types of collectibles on screen.