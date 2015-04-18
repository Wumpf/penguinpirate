using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Helpers
{
    class HermiteSpline
    {
        public static float TAP_MOVEMENT_SPEED = 0.01f;

        private Vector2 p_0, p_1, m_0, m_1;

        public Vector2 FinalMovementDirection {get{return m_1;}}

        public HermiteSpline(Vector2 start, Vector2 startDirection, Vector2 end, Vector2 endDirection)
        {
            p_0 = start;
            p_1 = end;
            m_0 = startDirection;
            m_1 = endDirection;
        }

        // Standard spline if no goal direction is given.
        public HermiteSpline(Vector2 start, Vector2 startDirection, Vector2 end)
        {
            p_0 = start;
            p_1 = end;
            m_0 = startDirection;
            m_1 = (end - start).normalized * TAP_MOVEMENT_SPEED;
        }

        public Vector2 EvaluateAt(float t)
        {
            // p(t) = (2t^3-3t^2+1)p_0 + (t^3-2t^2+t)m_0 + (-2t^3+3t^2)p_1 +(t^3-t^2)m_1 
            float t_square = t * t;
            float t_cube = t_square * t;

            return (2 * t_cube - 3 * t_square + 1) * p_0 + (t_cube - 2 * t_square + t) * m_0 + (-2 * t_cube + 3 * t_square + 1) * p_1 + (t_cube - t_square) * m_1;
        }
    }
}
