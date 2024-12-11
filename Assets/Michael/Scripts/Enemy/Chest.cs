using Michael.Scripts.Controller;
using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.Rendering;

namespace Michael.Scripts.Enemy
{
   public class Chest : MonoBehaviour
   {
      public int  ChestGold;
      [SerializeField]  private GameObject _buttonPrefabs;
      [SerializeField]  private GameObject _button;
      
      private void Start()
      {
         Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10, gameObject.transform.position.z);
        _button =  Instantiate(_buttonPrefabs, GameManager.Instance._mainCamera.WorldToScreenPoint(pos) , Quaternion.identity, GameManager.Instance._canvas.transform);
        _button.transform.SetAsFirstSibling();
      }

      public void RecoverGold()
      {
         //animation piece 
         //animation coffre qui s'ouvre 
         // 
         PlayerData.Instance.CurrentGold += ChestGold;
         ChestGold = 0;
         Destroy(gameObject);
         Destroy(_button);
         Debug.Log("Gold recovered");
      }
   }
}
