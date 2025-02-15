using System;
using System.Collections.Generic;

namespace UnityTools.Pathfinding {
    public class PriorityQueue<T> where T : IComparable<T> {
        private readonly List<(T item, float priority)> _heap = new();
        private readonly Dictionary<T, int> _positionMap = new(); // Contains 최적화

        public int Count => _heap.Count;

        public void Clear() {
            _heap.Clear();
            _positionMap.Clear();
        }

        public void Enqueue(T item, float priority) {
            if (_positionMap.ContainsKey(item)) return; // 중복 삽입 방지
            _heap.Add((item, priority));
            _positionMap[item] = _heap.Count - 1;
            HeapifyUp(_heap.Count - 1);
        }

        public T Dequeue() {
            if (_heap.Count == 0)
                throw new InvalidOperationException();

            T minItem = _heap[0].item;
            _positionMap.Remove(minItem);

            if (_heap.Count > 1) {
                _heap[0] = _heap[^1]; // 마지막 요소를 루트로 이동
                _positionMap[_heap[0].item] = 0;
            }

            _heap.RemoveAt(_heap.Count - 1);

            HeapifyDown(0);
            return minItem;
        }

        public bool Contains(T item) {
            return _positionMap.ContainsKey(item); // O(1) 탐색
        }

        private void HeapifyUp(int index) {
            while (index > 0) {
                int parent = (index - 1) / 2;
                if (_heap[index].priority >= _heap[parent].priority) break;

                Swap(index, parent);
                index = parent;
            }
        }

        private void HeapifyDown(int index) {
            while (true) {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int smallest = index;

                if (leftChild < _heap.Count && _heap[leftChild].priority < _heap[smallest].priority)
                    smallest = leftChild;
                if (rightChild < _heap.Count && _heap[rightChild].priority < _heap[smallest].priority)
                    smallest = rightChild;

                if (smallest == index) break;

                Swap(index, smallest);
                index = smallest;
            }
        }

        private void Swap(int i, int j) {
            (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
            _positionMap[_heap[i].item] = i;
            _positionMap[_heap[j].item] = j;
        }
    }
}