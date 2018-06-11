using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlotTrigger : MonoBehaviour,IListener {
    
    public bool activePropsTrigger;
    public List<TriggerPlot> plotList = new List<TriggerPlot>();

	int index;
    bool active = true;

	private void Awake()
	{
		Mediator.AddListener(this, "updateOwnProps");
	}

	private void OnDestroy()
	{
		Mediator.RemoveListener(this);
	}

	public void OnNotify(string notify, object args)
	{
		switch (notify)
		{
			case "updateOwnProps":
                if (active == false)
                    return;
                if (activePropsTrigger == false)
                    return;
				Args arg = args as Args;
				Props activeProps = arg.args[1] as Props;
				if (activeProps == null)
					return;
                if (plotList[index].propsId == activeProps.id)
				{
                    Mediator.SendMassage("triggerPlot", new Args(plotList[index].plotId, plotList[index].onFinish));
					Mediator.SendMassage("setActiveProps", null);
				}
				break;
		}
	}

    public void TriggerPlot(int plotId){
        if (active == false)
            return;
        TriggerPlot plot = plotList.Find((obj) => obj.plotId == plotId);
        Mediator.SendMassage("triggerPlot",new Args(plot.plotId,plot.onFinish));
    }

    public void TriggerPlot(){
        TriggerPlot(plotList[index].plotId);
    }

	public void GoNext()
	{
		index++;
	}

	public void GoTo(int index)
	{
		this.index = index;
	}

    public void SetActive(bool active){
        this.active = active;
    }

    public void ActivePropsTrigger(bool active){
        activePropsTrigger = active;
    }
}
