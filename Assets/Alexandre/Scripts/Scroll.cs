using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexandre
{
    public class ScrollingSystem : MonoBehaviour
    {
        public GameObject[] SlidePrefabs;
        public float FocusedSlideScale = 1f;
        public float UnfocusedSlideScale = 0.5f;
        public float Damping = 1f;
        public float SnapTime = 1f;
        public float AlphaOutOrder = 1;
        public float offset = 0f;

        private int length;
        private GameObject[] Slides;
        private Vector2 originalScale;
        private GameObject ToBeSnapSlide = null;
        private Vector2[] originalPosArrangement;
        private Vector2[] originalSclArrangement;

        void Awake()
        {
            length = SlidePrefabs.Length;
            screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            CreateSlides();
            originalScale = Slides[0].transform.localScale;
            //for saving the original Scale sizes of all our slides
            for (int i = 0; i < length; i++)
            {
                originalSclArrangement[i] = Slides[i].transform.localScale;
            }
        }

        void Start()
        {
        }

        #region TouchControl_Variables

        Vector2 touchPosition;
        Vector2 initialTouchPosition;
        bool inBwy;
        bool isDrawerGrabbed = false;

        #endregion

        Vector2 screenSize;
        float speed;
        float timeTakenToDrag;
        float TEMPspeed;
        bool TEMPsimulateInertia = false;
        bool TEMPisSnap = false;
        float retardation = 0;
        float displacement = 0;

        void Update()
        {
            #region TouchControl

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                inBwy = touchPosition.x <= originalScale.x * 0.5f && touchPosition.x >= -originalScale.x * 0.5f;

                if (!isDrawerGrabbed && inBwy && touch.phase == TouchPhase.Began)
                {
                    //All the code , when the Drawer is just grabbed by the user , goes here
                    UpdateOriginalArrangement();
                    initialTouchPosition = touchPosition;
                    isDrawerGrabbed = true;
                    timeTakenToDrag = 0;
                }

                if (isDrawerGrabbed && touch.phase == TouchPhase.Moved)
                {
                    //All the code , when the Drawer is being dragged by the user , goes here
                    Vector2 displacementVector = touchPosition - initialTouchPosition;
                    MoveSlides(new Vector2(0, displacementVector.y));
                    timeTakenToDrag += Time.deltaTime;
                }

                if (isDrawerGrabbed && touch.phase == TouchPhase.Ended)
                {
                    //All the code , when the Drawer is unGrabbed by the user , goes here
                    speed = (touchPosition.y - initialTouchPosition.y) / timeTakenToDrag;
                    isDrawerGrabbed = false;
                    TEMPspeed = speed;
                    TEMPsimulateInertia = true;
                    TEMPisSnap = false;
                }
            }

            #endregion

            if (TEMPsimulateInertia && speed != 0)
                SimulateInertia();
            if (!TEMPisSnap && Mathf.Abs(speed) <= 0.01f)
            {
                SnapInitialize();
                TEMPisSnap = true;
            }

            if (TEMPisSnap)
                Snap();
            UpdateSlidesWithSpeed();
            UpdateSlideScale();
        }

        void CreateSlides()
        {
            Slides = new GameObject[length];
            originalPosArrangement = new Vector2[length];
            originalSclArrangement = new Vector2[length];
            for (int i = 0; i < length; i++)
            {
                Slides[i] = Instantiate(SlidePrefabs[i]) as GameObject;
                Slides[i].transform.position = new Vector2(0, Slides[i].transform.localScale.y * i + offset);
                Slides[i].transform.SetParent(this.transform);
            }
        }

        void MoveSlides(Vector2 displacementVector)
        {
            for (int i = 0; i < length; i++)
                Slides[i].transform.position = displacementVector + originalPosArrangement[i];
        }

        void UpdateOriginalArrangement()
        {
            for (int i = 0; i < length; i++)
                originalPosArrangement[i] = Slides[i].transform.position;
        }

        void UpdateSlideScale()
        {
            for (int i = 0; i < length; i++)
            {
                GameObject obj = Slides[i];
                float Ivalue = InterpolateScale(obj.transform.position.y);
                obj.transform.localScale = Ivalue * originalSclArrangement[i];
                SpriteRenderer _RENDERER = obj.GetComponent<SpriteRenderer>();
                if (_RENDERER)
                    _RENDERER.color = new Color(_RENDERER.color.r, _RENDERER.color.g, _RENDERER.color.b,
                        Mathf.Pow(Mathf.Clamp01(Ivalue), AlphaOutOrder));
            }
        }

        float InterpolateScale(float _Screencoordinates)
        {
            float t = Mathf.Abs(_Screencoordinates) / (screenSize.y * 0.5f);
            return UnfocusedSlideScale * t + FocusedSlideScale * (1 - t);
        }

        void FindTobeSnappedSlide()
        {
            int TEMPindex = 0;
            float TEMPvalue1 = Mathf.Abs(Slides[0].transform.position.y);
            for (int i = 0; i < length - 1; i++)
            {
                float TEMPvalue2 = Mathf.Abs(Slides[i + 1].transform.position.y);
                if (TEMPvalue1 > TEMPvalue2)
                {
                    TEMPvalue1 = TEMPvalue2;
                    TEMPindex = i + 1;
                }
            }

            ToBeSnapSlide = Slides[TEMPindex];
        }

        void SnapInitialize()
        {
            FindTobeSnappedSlide();
            displacement = ToBeSnapSlide.transform.position.y;
            retardation = 2 * displacement / (SnapTime * SnapTime);
        }

        void Snap()
        {
            if (!ToBeSnapSlide) return;
            speed -= retardation * Time.deltaTime;
            if (Mathf.Abs(ToBeSnapSlide.transform.position.y) < 0.15f)
            {
                TEMPisSnap = false;
                speed = 0;
            }
        }

        void SimulateInertia()
        {
            speed += (-Damping * Time.deltaTime * Mathf.Abs(speed) / speed);
            if (TEMPspeed * speed < 0)
            {
                speed = 0;
                TEMPsimulateInertia = false;
                return;
            }

            TEMPspeed = speed;
        }

        void UpdateSlidesWithSpeed()
        {
            foreach (GameObject obj in Slides)
                obj.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
    }
}