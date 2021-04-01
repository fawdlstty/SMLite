package cn.fawdlstty.smlite.internface;

public interface IAction_sta<TState extends Enum, TTrigger extends Enum> {
    public void call (TState state, TTrigger trigger, Object[] args) throws Exception;
}
