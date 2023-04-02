using System.Collections;
using System.Collections.Generic;


namespace Game.Tests
{
    // attack: 6
    // jump: 12
    // landing: 0
    enum AnimState
    {
        Idle, Attack, Jump, Falling, Landing, Run, 
    }


    struct AnimFrame
    {
        public readonly AnimState State;
        public readonly int StateFrame;
        public readonly int TotalFrame;

        public AnimFrame(AnimState state, int stateFrame, int totalFrame)
        {
            State = state;
            StateFrame = stateFrame;
            TotalFrame = totalFrame;
        }

        public override string ToString() => $"{State}.{StateFrame} : {TotalFrame}";
    }
    
    class StatesStream : IEnumerable<AnimFrame>
    {
        public readonly List<AnimState> _frames = new();
        
        public StatesStream AddIdle(int frames) => AddAnimState(AnimState.Idle, frames);
        
        public StatesStream AddRun(int frames) => AddAnimState(AnimState.Run, frames);
        
        public StatesStream AddLanding() => AddAnimState(AnimState.Landing, 1);
        
        public StatesStream AddFalling(int frames) => AddAnimState(AnimState.Falling, frames);
        
        public StatesStream AddJump() => AddAnimState(AnimState.Jump, 12);
        
        public StatesStream AddAttack() => AddAnimState(AnimState.Attack, 6);

        private StatesStream AddAnimState(AnimState state, int frames)
        {
            for (int i = 0; i < frames; i++)
                _frames.Add(state);

            return this;
        }

        public IEnumerator<AnimFrame> GetEnumerator()
        {
            if (_frames.Count == 0)
                yield break;

            AnimState last = _frames[0];
            int total = 0;
            int current = 0;
            
            while (true)
            {
                yield return new AnimFrame(last, current, total);
                total += 1;
                current += 1;

                AnimState next = _frames[total % _frames.Count];

                if (next != last)
                {
                    current = 0;
                    last = next;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}