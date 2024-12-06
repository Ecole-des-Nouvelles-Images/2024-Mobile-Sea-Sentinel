using Michael.Scripts.Controller;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.Rendering;

namespace Michael.Scripts.Enemy
{
   public class Chest : MonoBehaviour
   {
      public int  ChestGold;
      public GameObject buttons;

      
      private void Start()
      {
         Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10, gameObject.transform.position.z);
        GameObject button =  Instantiate(buttons, GameManager.Instance._mainCamera.WorldToScreenPoint(pos) , Quaternion.identity, GameManager.Instance._canvas.transform);
        button.transform.SetAsFirstSibling();
      }

      public void RecoverGold()
      {
         //animation piece 
         //animation coffre qui s'ouvre 
         // 
         PlayerData.Instance.CurrentGold += ChestGold;
         ChestGold = 0;
         Destroy(gameObject);
         Destroy(buttons);
         Debug.Log("Gold recovered");
      }
   }
}
