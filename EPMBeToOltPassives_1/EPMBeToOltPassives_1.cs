/*
****************************************************************************
*  Copyright (c) 2022,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

dd/mm/2022	1.0.0.1		DPR, Skyline	Initial version
dd/mm/2022	1.0.0.2		DPR, Skyline	Upgrade NuGets Interapp
****************************************************************************
*/

using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using Skyline.DataMiner.Automation;
using Skyline.DataMiner.Core.InterAppCalls.Common.CallBulk;
using Skyline.DataMiner.Core.InterAppCalls.Common.CallSingle;
using Skyline.DataMiner.Core.InterAppCalls.Common.Shared;

/// <summary>
/// DataMiner Script Class.
/// </summary>
public class Script
{
	/// <summary>
	/// The Script entry point.
	/// </summary>
	/// <param name="engine">Link with SLAutomation process.</param>
	public void Run(Engine engine)
	{
		Message message = new Message();
		string pairDetails = engine.GetScriptParam("CollectorList").Value;

		List<string> pairList = JsonConvert.DeserializeObject<List<string>>(pairDetails);
		IInterAppCall command = InterAppCallFactory.CreateNew();

		command.Messages.Add(message);

		foreach (string pair in pairList)
		{
			string[] split = pair.Split('/');
			string dmaId = split[0];
			string collectorId = split[1];
			command.Source = new Source("EPM ID PASSIVE FE");
			SendMessage(command, Convert.ToInt32(dmaId), Convert.ToInt32(collectorId));
		}
	}

	private static void SendMessage(IInterAppCall command, int dmaId, int connectorId)
	{
		command.ReturnAddress = new ReturnAddress(dmaId, connectorId, 9000001);
		command.Send(Engine.SLNetRaw, dmaId, connectorId, 9000000, new List<Type> { typeof(List<string>) });
	}
}