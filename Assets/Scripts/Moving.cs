using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cubes
{
    public class Moving : MonoBehaviour
    {
        [SerializeField]
        BezierCurve _BezierCurvePrefab;
        [SerializeField]
        private LayerMask LayerMask;
        [SerializeField]
        private float _Speed;
        [SerializeField]
        private List<Transform> _Cubes1;
        [SerializeField]
        private List<Transform> _Cubes2;

        public float _Duration;

        private Transform _CurrentDestination;
        private Transform _EndDestinationTransform;
        private float _Progress;
        private bool NeedAdditionalDestination = false;
        private Vector3 _StartPosition;
        private BezierCurve _BezierCurve;
        private int _CurveKoeff = 5;
        private void Awake()
        {
            _StartPosition = transform.position;
            _BezierCurve = Instantiate(_BezierCurvePrefab);
        }
        private Transform FindClosestCube(Transform hittedTransorm)
        {
            if (_Cubes1.Contains(hittedTransorm))
            {
                return _Cubes1.OrderBy(o => Vector3.Distance(hittedTransorm.position, o.position)).Where(o => o != hittedTransorm).First();
            }
            else
            {
                return _Cubes2.OrderBy(o => Vector3.Distance(hittedTransorm.position, o.position)).Where(o => o != hittedTransorm).First();
            }
        }

        IEnumerator Move()
        {
            _Progress = 0;

            while (transform.position != _CurrentDestination.position)
            {
                _Progress += Time.deltaTime * _Speed;
                Vector3 position = _BezierCurve.GetPoint(_Progress);
                transform.position = position;

                transform.LookAt(position + _BezierCurve.GetDirection(_Progress));
                yield return null;
            }
            transform.rotation = _CurrentDestination.transform.rotation;
        }
        private bool ChooseDestination(Transform closestToDestinationPosition, ref Transform currentDestination)
        {
            if (Vector3.Distance(transform.position, closestToDestinationPosition.position) < Vector3.Distance(transform.position, _EndDestinationTransform.position))
            {
                currentDestination = closestToDestinationPosition;
                return true;
            }
            else
            {
                currentDestination = _EndDestinationTransform;
                return false;
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.position = _StartPosition;
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, LayerMask))
                {
                    _EndDestinationTransform = hit.collider.gameObject.transform;
                    Transform closestToDestination = FindClosestCube(_EndDestinationTransform);

                    NeedAdditionalDestination = ChooseDestination(closestToDestination, ref _CurrentDestination);
                    _BezierCurve.points = new Vector3[4] { transform.position, new Vector3(transform.position.x + _CurveKoeff, transform.position.y, transform.position.z), _CurrentDestination.position + Vector3.up * _CurveKoeff, _CurrentDestination.position };

                    StartCoroutine(Move());
                }
            }
            if (NeedAdditionalDestination && transform.position == _CurrentDestination.position)
            {
                NeedAdditionalDestination = false;
                _CurrentDestination = _EndDestinationTransform;
                _BezierCurve.points = new Vector3[4] { transform.position, new Vector3(transform.position.x + _CurveKoeff, transform.position.y, transform.position.z), _CurrentDestination.position + Vector3.up * _CurveKoeff, _CurrentDestination.position };
                StartCoroutine(Move());
            }
        }
    }
}
