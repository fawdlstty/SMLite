package cn.fawdlstty.smlite.internface;

public interface IFunc_s<TState> {
    public TState call (TState state) throws Exception;
}
