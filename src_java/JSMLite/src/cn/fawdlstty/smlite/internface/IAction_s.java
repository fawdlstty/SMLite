package cn.fawdlstty.smlite.internface;

public interface IAction_s<TState extends Enum> {
    public void call (TState state) throws Exception;
}
