using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : WebClientService
{
  class AnimationContext
  {
    public string       TargetName;
    public List<string> AnimationList = new List<string>();
  }

  public void FetchAnimations()
  {
    Send("list_targets");
    Send("list_anims");
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public override void HandleMessage(WebSocketSharp.MessageEventArgs messageData)
  {
    throw new NotImplementedException();
  }
}
