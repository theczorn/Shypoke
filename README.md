# Shypoke
Goals: To slowly create a over-the-web Texas No Hold 'Em poker client iteratively over time.

LEGEND:
-V = Validate Code
-X = Completed

ITERATION 1 - Primitive Console (Unplayable State)
--Handle Betting Scheme [V]
--Handle Card Comparisons [X]
--Handle Game End [X]
--Handle 2 Person game ruleset [V]
--Handle Empty Deck merge [X]
--Implement Ace Low detection for straights/straight flushes
--ENSURE CYCLOMATIC COMPLEXITY lower than 10

ITERATION 2 - Local GUI Client (Unplayable State)
--Refactor into basic GUI {determine arch, MVC?}
--Map image assets
--Customize LinkedList for tracking players [X]

ITERATION 3 - Player Hosted Gamerooms over the Net (Playable State)
--Multithread Card Comparison
--allow for non-LAN play {determine arch}

ITERATION 4 - TBD, other card games rulesets, heavy architectural refactoring/optimization
