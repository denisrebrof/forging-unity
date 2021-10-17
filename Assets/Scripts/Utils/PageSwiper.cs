using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils
{
    public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler {

        private Vector3 panelLocation;
        public float percentThreshold = 0.2f;
        public float easing = 0.5f;
        public int totalPages = 1;
        private int currentPage = 1;

        private void Start(){
            panelLocation = transform.localPosition;
        }

        public void OnDrag(PointerEventData data) {
            float difference = data.pressPosition.x - data.position.x;
            transform.localPosition = panelLocation - new Vector3(difference, 0, 0);
        }

        public void OnEndDrag(PointerEventData data) {
            float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
            if(Mathf.Abs(percentage) >= percentThreshold){
                Vector3 newLocation = panelLocation;
                if(percentage > 0 && currentPage < totalPages){
                    currentPage++;
                    newLocation += new Vector3(-Screen.width, 0, 0);
                }else if(percentage < 0 && currentPage > 1){
                    currentPage--;
                    newLocation += new Vector3(Screen.width, 0, 0);
                }
                StartCoroutine(SmoothMove(transform.localPosition, newLocation, easing));
                panelLocation = newLocation;
            }else{
                StartCoroutine(SmoothMove(transform.localPosition, panelLocation, easing));
            }
        }

        IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds) {
            float t = 0f;
            while(t <= 1.0){
                t += Time.deltaTime / seconds;
                transform.localPosition = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
        }
    }
}