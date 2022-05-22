
At first I thought of developing this system by using the DDD approach and using CQRS to separate the command and query sections.
But I preferred to follow the KISS principle and not add complexity until I could find a solid reason to use each technique.
And I know that any pattern, if used in the wrong place, is anti-pattern itself.
The problem was just asking me to solve the problem, not the show-off of my knowledge and tools that I know.
So I choose the simplest method, with knowing the possible aspects of the question.

I use chain of response pattern for multi-step validation and move service of Controllers to another place and extract methods for cleaning codes.

The tests were written in the same style as before, and it can be said that they are in the integration test category, and the unit test could have been added, but since it was not seen in the system, I skipped writing it.

