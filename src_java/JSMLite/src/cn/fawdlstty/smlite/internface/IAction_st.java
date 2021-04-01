package cn.fawdlstty.smlite.internface;

public interface IAction_st<TState extends Enum, TTrigger extends Enum> {
    public void call (TState state, TTrigger trigger) throws Exception;
}
