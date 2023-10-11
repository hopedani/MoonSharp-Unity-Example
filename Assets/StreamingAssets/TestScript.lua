return function()
	SetText("Hi "..State.PlayerName.."! I'm a Lua Script!")
	ShowButtons("Hello", "Goodbye")
	
	coroutine.yield()
	
	if(State.ButtonSelected()==1) then
		SetText("Second Slide")
	else
		SetText("Other option")
	end
end
	