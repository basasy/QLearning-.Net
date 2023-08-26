# Q-Learning Algorithm with C#
The aim of this study is to present the work of the Q-Learning algorithm with a visual interface using C# language and Windows Form technologies.
## What is Q-Learning ?

Q-learning is a model-free reinforcement learning algorithm used for solving problems of repeated decision-making and learning. This algorithm enables an agent to interact with its environment to develop an optimal action strategy. Q-learning is particularly applicable to problems formulated within specific mathematical frameworks like Markov Decision Processes (MDP).

### How Q-learning Works

Q-learning involves learning a value function known as "Q-values" for state-action pairs. Q-values represent the expected cumulative reward or return for taking a specific action in a given state. The algorithm is named after the "Q" values, which are organized in a matrix. Each row in this matrix corresponds to a state, and each column corresponds to an action. The Q-value estimates the expected total reward or return the agent can achieve by performing a specific action in a certain state.

The basic flow of the Q-learning algorithm can be summarized as follows:

1. Initially, Q-values are initialized randomly.
2. The agent interacts with the environment, observing its current state, selecting an action, and performing it.
3. After the action, the agent transitions to a new state and receives a reward.
4. The agent updates the Q-value corresponding to the state-action pair. This update is based on the difference between the old Q-value and a combination of the obtained reward and the maximum Q-value of the future state.
5. Steps 2-4 are repeated; the agent continues to interact with the environment, and Q-values become increasingly accurate.
6. The algorithm implements the optimal strategy by selecting the action with the highest Q-value, starting from a given state.

Q-learning uses the Bellman equation and the Q-learning update rule to update Q-values:


![image](https://github.com/basasy/QLearning-.Net/assets/48106789/6b97e9eb-b662-4108-afb4-cbe8d1473016)


- Q(s, a): Q-value for state \(s\) and action \(a\).
- a: Learning rate, controlling the speed of Q-value updates.
- r: Immediate reward.
- Y*: Discount factor, determining how future rewards are weighted relative to present rewards.
- s': New state.


Q-learning leverages an agent's experiences to learn an optimal action strategy. However, for problems with large state and action spaces, the Q-values matrix can become extensive, requiring more sophisticated approaches.

## Project User Face

![Adsız yeni](https://github.com/basasy/QLearning-.Net/assets/48106789/3b8cf315-5810-4dc4-8ca1-29fea7035f37)
### Result 
![Adsız2Yeni](https://github.com/basasy/QLearning-.Net/assets/48106789/2a26808c-5ffe-4625-83fc-3d2853086c1d)

