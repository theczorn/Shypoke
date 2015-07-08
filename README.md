# Shypoke
Goals: To slowly create an over-the-web Texas No Hold 'Em poker client iteratively over time.


ITERATION 1 - Primitive Console (Unplayable State)
- [ ] Handle Betting Scheme											//Validate
- [X] Handle Card Comparisons 
- [X] Handle Game End Logic
- [ ] Handle 2 Person game rulset									//Validate
- [X] Handle Empty Deck merge
- [X] Implement Ace Low detecton for straights/straight flushes
- [ ] Ensure Cyclomatic Complexity lower than 10


ITERATION 2 - Local GUI Client (Unplayable State)
- [ ] Refactor into basic GUI {determine arch, MVC?}
- [ ] Find/Map image assets
- [X] Customize LinkedList for tracking players


ITERATION 3 - Player Hosted Gamerooms over the Net (Playable State)
- [ ] Multithread Card Comparison
- [ ] Allow for LAN/WAN play {determine arch}


ITERATION 4 - TBD, other card games rulesets, heavy architectural refactoring/optimization
