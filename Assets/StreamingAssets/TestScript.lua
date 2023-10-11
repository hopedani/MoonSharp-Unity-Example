local helloBranch = function()
	SetText("Thrid Slide")
	State.SetFlag("FirstButton", true)
	
	coroutine.yield()
	
	SetText("Last inner coroutine line!")
end

local main =  function()
	SetText("Hi "..State.PlayerName.."! I'm a Lua Script!")
	ShowButtons("Hello", "Goodbye")
	
	coroutine.yield()
	
	if(State.ButtonSelected==1) then
		SetText("Second Slide")
		coroutine.yield(helloBranch)
	else
		SetText("Other option")
		coroutine.yield()
	end
	
	if State.GetFlag("FirstButton") then
		SetText("Flag was set")
	end
end

return main
	