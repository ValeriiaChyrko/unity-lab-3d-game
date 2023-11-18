using System;
using System.Collections.Generic;
using System.Linq;

namespace Platformer._Project.Scripts.StateMachine
{
    public class StateMachine {
        private StateNode _current;
        private readonly Dictionary<Type, StateNode> _nodes = new(); // Dictionary to store state nodes.
        private readonly HashSet<ITransition> _anyTransitions = new(); // Set to store transitions applicable to any state.

        // Update method for general state updates.
        public void Update() {
            var transition = GetTransition();
            if (transition != null) 
                ChangeState(transition.To);
            
            _current.State?.Update(); // Update the current state.
        }
        
        // FixedUpdate method for fixed time step state updates.
        public void FixedUpdate() {
            _current.State?.FixedUpdate(); // FixedUpdate for the current state.
        }

        // Set the initial state of the state machine.
        public void SetState(IState state) {
            _current = _nodes[state.GetType()];
            _current.State?.OnEnter(); // Invoke the OnEnter method of the current state.
        }

        // Change state based on a given state.
        private void ChangeState(IState state) {
            if (state == _current.State) return; // If the new state is the same as the current state, do nothing.
            
            var previousState = _current.State;
            var nextState = _nodes[state.GetType()].State;
            
            previousState?.OnExit(); // Invoke the OnExit method of the previous state.
            nextState?.OnEnter(); // Invoke the OnEnter method of the next state.
            _current = _nodes[state.GetType()]; // Update the current state.
        }

        // Determine the transition to a new state.
        private ITransition GetTransition() {
            foreach (var transition in _anyTransitions.Where(transition => transition.Condition.Evaluate()))
                return transition; // Return the first applicable transition from any state.

            return _current.Transitions.FirstOrDefault(transition => transition.Condition.Evaluate()); // Return the first applicable transition from the current state.
        }

        // Add a transition between two specific states.
        public void AddTransition(IState from, IState to, IPredicate condition) {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }
        
        // Add a transition applicable to any state.
        public void AddAnyTransition(IState to, IPredicate condition) {
            _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        // Get or add a state node to the dictionary.
        private StateNode GetOrAddNode(IState state) {
            var node = _nodes.GetValueOrDefault(state.GetType());

            if (node != null) return node;
            
            node = new StateNode(state);
            _nodes.Add(state.GetType(), node);
            return node;
        }

        // Represents a state node in the state machine.
        private class StateNode {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }
            
            public StateNode(IState state) {
                State = state;
                Transitions = new HashSet<ITransition>();
            }
            
            // Add a transition to the state node.
            public void AddTransition(IState to, IPredicate condition) {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}
